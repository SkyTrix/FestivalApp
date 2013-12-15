using FestivalApp.Utilities;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class GenreManager : ObservableObject
    {
        #region "Properties"
        private static readonly GenreManager _instance = new GenreManager();

        private GenreManager()
        {
        }

        public static GenreManager Instance
        {
            get
            {
                return _instance;
            }
        }

        private ObservableCollection<Genre> _genres;
        public ObservableCollection<Genre> Genres
        {
            get
            {
                if (_genres == null)
                    _genres = GetGenres();

                return _genres;
            }
            set { _genres = value; OnPropertyChanged("Genres"); }
        }
        #endregion

        private ObservableCollection<Genre> GetGenres()
        {
            try
            {
                string query = "SELECT [ID], [Name] FROM [Genres]";
                DbDataReader reader = Database.GetData(query);

                return GetResults(reader);
            }
            catch (Exception)
            {
                return new ObservableCollection<Genre>();
            }
        }

        public static ObservableCollection<Genre> GetGenresForBand(Band band)
        {
            try
            {
                ObservableCollection<Genre> list = new ObservableCollection<Genre>();

                string query = "SELECT [ID], [Name] FROM [Genres] JOIN [Band_Genre] ON [Genres].[ID] = [Band_Genre].[GenreID] WHERE [Band_Genre].[BandID] = @BandID";
                DbParameter idParam = Database.CreateParameter("@BandID", band.ID);
                DbDataReader reader = Database.GetData(query, idParam);

                return GetResults(reader);
            }
            catch (Exception)
            {
                return new ObservableCollection<Genre>();
            }
        }

        public void SetGenresForBand(Band band, IEnumerable<Genre> genres)
        {
            DbTransaction transaction = Database.BeginTransaction();

            try
            {
                // Delete all links between genres and the band
                string sql = "DELETE FROM [Band_Genre] WHERE [BandID] = @ID";

                Database.ModifyData(transaction, sql,
                    Database.CreateParameter("@ID", band.ID)
                );

                // Add links between genres and the band
                foreach (Genre genre in genres)
                {
                    string query = "INSERT INTO [Band_Genre] ([BandID], [GenreID]) VALUES (@BandID, @GenreID)";
                    Database.ModifyData(transaction, query,
                        Database.CreateParameter("@BandID", band.ID),
                        Database.CreateParameter("@GenreID", genre.ID)
                    );
                }

                transaction.Commit();

                // We cannot release the connection in the middle of a transaction
                Database.ReleaseConnection(transaction.Connection);
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        private static ObservableCollection<Genre> GetResults(DbDataReader reader)
        {
            ObservableCollection<Genre> list = new ObservableCollection<Genre>();
            while (reader.Read())
            {
                list.Add(CreateGenre(reader));
            }
            reader.Close();

            return list;
        }

        private static Genre CreateGenre(IDataRecord row)
        {
            Genre genre = new Genre();
            genre.ID = !Convert.IsDBNull(row["ID"]) ? row["ID"].ToString() : null;
            genre.Name = !Convert.IsDBNull(row["Name"]) ? row["Name"].ToString() : string.Empty;

            return genre;
        }

        public void AddGenre(Genre genre)
        {
            try
            {
                string sql = "INSERT INTO [Genres] ([Name])";
                sql += " VALUES (@Name)";

                Database.ModifyData(sql,
                    Database.CreateParameter("@Name", genre.Name)
                );

                // Refresh data to retrieve ID of newly added item
                Genres = GetGenres();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EditGenre(Genre genre)
        {
            try
            {
                string sql = "UPDATE [Genres] SET [Name] = @Name";
                sql += " WHERE [ID] = @ID";

                Database.ModifyData(sql,
                    Database.CreateParameter("@ID", genre.ID),
                    Database.CreateParameter("@Name", genre.Name)
                );

                Genres = GetGenres();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteGenre(Genre genre)
        {
            try
            {
                string sql = "DELETE FROM [Genres] WHERE [ID] = @ID";

                Database.ModifyData(sql,
                    Database.CreateParameter("@ID", genre.ID)
                );

                Genres = GetGenres();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
