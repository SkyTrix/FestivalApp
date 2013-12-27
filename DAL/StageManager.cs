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
    public class StageManager : ObservableObject
    {
        #region "Properties"
        private static readonly StageManager _instance = new StageManager();

        private StageManager()
        {
        }

        public static StageManager Instance
        {
            get
            {
                return _instance;
            }
        }

        private ObservableCollection<Stage> _stages;
        public ObservableCollection<Stage> Stages
        {
            get
            {
                if (_stages == null)
                    _stages = GetStages();

                return _stages;
            }
            set { _stages = value; OnPropertyChanged("Stages"); }
        }
        #endregion

        // Do not use this method in WPF app
        // Only to be used when direct access to data from db is needed (website, API)
        public static ObservableCollection<Stage> GetStages()
        {
            try
            {
                string query = "SELECT [ID], [Name] FROM [Stages]";
                DbDataReader reader = Database.GetData(query);

                return GetResults(reader);
            }
            catch (Exception)
            {
                return new ObservableCollection<Stage>();
            }
        }

        public static Stage GetStageByID(string id)
        {
            try
            {
                string query = "SELECT [ID], [Name] FROM Stages WHERE ID = @ID";

                DbDataReader reader = Database.GetData(query,
                    Database.CreateParameter("@ID", id)
                );
                ObservableCollection<Stage> stages = GetResults(reader);

                return stages.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static ObservableCollection<Stage> GetResults(DbDataReader reader)
        {
            ObservableCollection<Stage> list = new ObservableCollection<Stage>();
            while (reader.Read())
            {
                list.Add(CreateStage(reader));
            }
            reader.Close();

            return list;
        }

        public bool StageUsedInLineup(Stage stage)
        {
            try
            {
                string query = "SELECT COUNT(*) AS [COUNT] FROM [LineUp] WHERE [Stage] = @ID";

                DbDataReader reader = Database.GetData(query,
                    Database.CreateParameter("@ID", stage.ID)
                );

                if (reader.HasRows)
                {
                    reader.Read();
                    bool usedInLineUp = (int)reader["COUNT"] > 0;
                    reader.Close();

                    return usedInLineUp;
                }

                reader.Close();

                return false;
            }
            catch (Exception)
            {
                // Don't try to delete if check fails
                return true;
            }
        }

        private static Stage CreateStage(IDataRecord row)
        {
            Stage stage = new Stage();
            stage.ID = !Convert.IsDBNull(row["ID"]) ? (int)row["ID"] : -1;
            stage.Name = !Convert.IsDBNull(row["Name"]) ? row["Name"].ToString() : string.Empty;

            return stage;
        }

        public void AddStage(Stage stage)
        {
            try
            {
                string sql = "INSERT INTO [Stages] ([Name]) VALUES (@Name)";

                Database.ModifyData(sql,
                    Database.CreateParameter("@Name", stage.Name)
                );

                // Refresh data to retrieve ID of newly added item
                Stages = GetStages();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EditStage(Stage stage)
        {
            try
            {
                string sql = "UPDATE [Stages] SET [Name] = @Name";
                sql += " WHERE [ID] = @ID";

                Database.ModifyData(sql,
                    Database.CreateParameter("@ID", stage.ID),
                    Database.CreateParameter("@Name", stage.Name)
                );

                Stages = GetStages();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteStage(Stage stage)
        {
            try
            {
                string sql = "DELETE FROM [Stages] WHERE [ID] = @ID";

                Database.ModifyData(sql,
                    Database.CreateParameter("@ID", stage.ID)
                );

                Stages = GetStages();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
