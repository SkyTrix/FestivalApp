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
    class StageManager : ObservableObject
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

        private ObservableCollection<Stage> GetStages()
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
            string query = "SELECT [ID], [Name] FROM Stages WHERE ID = @ID";
            DbParameter idPar = Database.CreateParameter("@ID", id);

            DbDataReader reader = Database.GetData(query, idPar);

            ObservableCollection<Stage> stages = GetResults(reader);
            if (stages.Count > 0)
                return stages[0];

            return null;
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

        private static Stage CreateStage(IDataRecord row)
        {
            Stage stage = new Stage();
            stage.ID = !Convert.IsDBNull(row["ID"]) ? row["ID"].ToString() : null;
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
    }
}
