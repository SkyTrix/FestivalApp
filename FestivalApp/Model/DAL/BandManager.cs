using FestivalApp.Utilities;
using FestivalApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalApp.Model.DAL
{
    class BandManager : ObservableObject
    {
        #region "Properties"
        private static readonly BandManager _instance = new BandManager();

        private BandManager()
        {
        }

        public static BandManager Instance
        {
            get
            {
                return _instance;
            }
        }

        private ObservableCollection<Band> _bands;
        public ObservableCollection<Band> Bands
        {
            get
            {
                if (_bands == null)
                    _bands = GetBands();

                return _bands;
            }
            set { _bands = value; OnPropertyChanged("Bands"); }
        }
        #endregion

        private ObservableCollection<Band> GetBands()
        {
            try
            {
                string query = "SELECT [ID], [Name], [PictureURL], [Description], [Twitter], [Facebook] FROM [Bands]";
                DbDataReader reader = Database.GetData(query);

                return GetResults(reader);
            }
            catch (Exception)
            {
                return new ObservableCollection<Band>();
            }
        }

        public static Band GetBandByID(string id)
        {
            try
            {
                string query = "SELECT [ID], [Name], [PictureURL], [Description], [Twitter], [Facebook] FROM [Bands] WHERE ID = @ID";
                DbParameter idPar = Database.CreateParameter("@ID", id);

                DbDataReader reader = Database.GetData(query, idPar);

                ObservableCollection<Band> bands = GetResults(reader);
                if (bands.Count > 0)
                    return bands[0];

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static ObservableCollection<Band> GetResults(DbDataReader reader)
        {
            ObservableCollection<Band> list = new ObservableCollection<Band>();
            while (reader.Read())
            {
                list.Add(CreateBand(reader));
            }
            reader.Close();

            return list;
        }

        private static Band CreateBand(IDataRecord row)
        {
            Band band = new Band();
            band.ID = !Convert.IsDBNull(row["ID"]) ? row["ID"].ToString() : null;
            band.Name = !Convert.IsDBNull(row["Name"]) ? row["Name"].ToString() : string.Empty;
            band.PictureURL = !Convert.IsDBNull(row["PictureURL"]) ? row["PictureURL"].ToString() : null;
            band.Description = !Convert.IsDBNull(row["Description"]) ? row["Description"].ToString() : string.Empty;
            band.Twitter = !Convert.IsDBNull(row["Twitter"]) ? row["Twitter"].ToString() : string.Empty;
            band.Facebook = !Convert.IsDBNull(row["Facebook"]) ? row["Facebook"].ToString() : string.Empty;

            band.Genres = GenreManager.GetGenresForBand(band);

            return band;
        }

        public void AddBand(Band band)
        {
            try
            {
                string sql = "INSERT INTO [Bands] ([Name], [PictureURL], [Description], [Twitter], [Facebook])";
                sql += " VALUES (@Name, @PictureURL, @Description, @Twitter, @Facebook)";

                Database.ModifyData(sql,
                    Database.CreateParameter("@Name", band.Name),
                    Database.CreateParameter("@PictureURL", band.PictureURL),
                    Database.CreateParameter("@Description", band.Description),
                    Database.CreateParameter("@Twitter", band.Twitter),
                    Database.CreateParameter("@Facebook", band.Facebook)
                );

                // Refresh data to retrieve ID of newly added item
                Bands = GetBands();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EditBand(Band band)
        {
            try
            {
                string sql = "UPDATE [Bands] SET [Name] = @Name, [PictureURL] = @PictureURL, [Description] = @Description, [Twitter] = @Twitter, [Facebook] = @Facebook";
                sql += " WHERE [ID] = @ID";

                Database.ModifyData(sql,
                    Database.CreateParameter("@ID", band.ID),
                    Database.CreateParameter("@Name", band.Name),
                    Database.CreateParameter("@PictureURL", band.PictureURL),
                    Database.CreateParameter("@Description", band.Description),
                    Database.CreateParameter("@Twitter", band.Twitter),
                    Database.CreateParameter("@Facebook", band.Facebook)
                );
                Bands = GetBands();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RefreshData()
        {
            Bands = GetBands();
        }
    }
}
