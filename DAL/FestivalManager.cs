using FestivalApp.Utilities;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FestivalManager : ObservableObject
    {
        #region "Properties"
        private static readonly FestivalManager _instance = new FestivalManager();

        private FestivalManager()
        {
        }

        public static FestivalManager Instance
        {
            get
            {
                return _instance;
            }
        }

        private Festival _festival;
        public Festival Festival
        {
            get
            {
                if (_festival == null)
                    _festival = GetFestival();

                return _festival;
            }
            set { _festival = value; OnPropertyChanged("Festival"); }
        }
        #endregion

        private static Festival GetFestival()
        {
            if (GetFestivalFromDB() == null)
            {
                InsertDemoFestival();
            }

            // Get from DB to receive the ID
            return GetFestivalFromDB();
        }

        private static Festival GetFestivalFromDB()
        {
            try
            {
                string query = "SELECT [ID], [Name], [StartDate], [EndDate] FROM [Festival]";
                DbDataReader reader = Database.GetData(query);
                if (reader.HasRows)
                {
                    reader.Read();
                    return CreateFestival(reader);
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static void InsertDemoFestival()
        {
            Festival festival = new Festival();
            festival.Name = "Howest Rock";
            festival.StartDate = DateTime.Today;
            festival.EndDate = festival.StartDate.AddDays(4);

            InsertFestival(festival);
        }

        private static void InsertFestival(Festival festival)
        {
            try
            {
                string sql = "INSERT INTO [Festival] ([Name], [StartDate], [EndDate])";
                sql += " VALUES(@Name, @StartDate, @EndDate)";

                Database.ModifyData(sql,
                    Database.CreateParameter("@Name", festival.Name),
                    Database.CreateParameter("@StartDate", festival.StartDate),
                    Database.CreateParameter("@EndDate", festival.EndDate)
                );
            }
            catch (Exception)
            {
            }
        }

        public void SaveFestival()
        {
            try
            {
                string sql = "UPDATE [Festival] SET [Name] = @Name, [StartDate] = @StartDate, [EndDate] = @EndDate";
                sql += " WHERE [ID] = @ID";

                Database.ModifyData(sql,
                    Database.CreateParameter("@ID", Festival.ID),
                    Database.CreateParameter("@Name", Festival.Name),
                    Database.CreateParameter("@StartDate", Festival.StartDate),
                    Database.CreateParameter("@EndDate", Festival.EndDate)
                );
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static Festival CreateFestival(IDataRecord row)
        {
            Festival festival = new Festival();
            festival.ID = !Convert.IsDBNull(row["ID"]) ? row["ID"].ToString() : null;
            festival.Name = !Convert.IsDBNull(row["Name"]) ? row["Name"].ToString() : string.Empty;
            festival.StartDate = !Convert.IsDBNull(row["StartDate"]) ? (DateTime)row["StartDate"] : DateTime.Now;
            festival.EndDate = !Convert.IsDBNull(row["EndDate"]) ? (DateTime)row["EndDate"] : DateTime.Now;

            return festival;
        }
    }
}
