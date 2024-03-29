﻿using FestivalApp.Utilities;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class LineUpManager : ObservableObject
    {
        #region "Properties"
        private static readonly LineUpManager _instance = new LineUpManager();
        
        private LineUpManager()
        {
        }

        public static LineUpManager Instance
        {
            get
            {
                return _instance;
            }
        }

        private ObservableCollection<LineUpItem> _lineUpItems;
        public ObservableCollection<LineUpItem> LineUpItems
        {
            get
            {
                if (_lineUpItems == null)
                    _lineUpItems = GetLineUpItems();

                return _lineUpItems;
            }
            set { _lineUpItems = value; OnPropertyChanged("LineUpItems"); }
        }
        #endregion

        // Do not use this method in WPF app
        // Only to be used when direct access to data from db is needed (website, API)
        public static ObservableCollection<LineUpItem> GetLineUpItems()
        {
            try
            {
                string query = "SELECT [ID], [Date], [StartTime], [EndTime], [Stage], [Band] FROM [LineUp]";
                DbDataReader reader = Database.GetData(query);

                return GetResults(reader);
            }
            catch (Exception)
            {
                return new ObservableCollection<LineUpItem>();
            }
        }

        public static LineUpItem GetLineUpItemByID(string id)
        {
            try
            {
                string query = "SELECT [ID], [Date], [StartTime], [EndTime], [Stage], [Band] FROM [LineUp] WHERE ID = @ID";

                DbDataReader reader = Database.GetData(query,
                    Database.CreateParameter("@ID", id)
                );
                ObservableCollection<LineUpItem> items = GetResults(reader);

                return items.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static ObservableCollection<LineUpItem> GetResults(DbDataReader reader)
        {
            ObservableCollection<LineUpItem> list = new ObservableCollection<LineUpItem>();
            while (reader.Read())
            {
                list.Add(CreateLineUpItem(reader));
            }
            reader.Close();

            return list;
        }

        private static LineUpItem CreateLineUpItem(IDataRecord row)
        {
            LineUpItem lineUpItem = new LineUpItem();
            lineUpItem.ID = !Convert.IsDBNull(row["ID"]) ? (int)row["ID"] : -1;
            lineUpItem.Date = !Convert.IsDBNull(row["Date"]) ? (DateTime)row["Date"] : DateTime.Now;
            lineUpItem.StartTime = !Convert.IsDBNull(row["StartTime"]) ? (string)row["StartTime"] : null;
            lineUpItem.EndTime = !Convert.IsDBNull(row["EndTime"]) ? (string)row["EndTime"] : null;
            lineUpItem.Stage = StageManager.GetStageByID(row["Stage"].ToString());
            lineUpItem.Band = BandManager.GetBandByID(row["Band"].ToString());

            return lineUpItem;
        }

        public bool LineUpItemOverlapsWithExistingItems(LineUpItem item)
        {
            DateTime startTime = LineUpItem.DateAndTimeStringToDateTime(item.Date, item.StartTime);
            DateTime endTime = LineUpItem.DateAndTimeStringToDateTime(item.Date, item.EndTime);

            foreach (LineUpItem lineUpItem in LineUpItems.ToList().FindAll(x => x.Stage.ID == item.Stage.ID))
            {
                if (item.ID == lineUpItem.ID)
                    continue;

                DateTime start = LineUpItem.DateAndTimeStringToDateTime(lineUpItem.Date, lineUpItem.StartTime);
                DateTime end = LineUpItem.DateAndTimeStringToDateTime(lineUpItem.Date, lineUpItem.EndTime);

                if ((startTime <= end) && (endTime >= start))
                {
                    return true;
                }
            }

            return false;
        }

        public void AddLineUpItem(LineUpItem item)
        {
            try
            {
                string sql = "INSERT INTO [LineUp] ([Date], [StartTime], [EndTime], [Stage], [Band])";
                sql += " VALUES (@Date, @StartTime, @EndTime, @Stage, @Band)";

                Database.ModifyData(sql,
                    Database.CreateParameter("@Date", item.Date),
                    Database.CreateParameter("@StartTime", item.StartTime),
                    Database.CreateParameter("@EndTime", item.EndTime),
                    Database.CreateParameter("@Stage", item.Stage.ID),
                    Database.CreateParameter("@Band", item.Band.ID)
                );

                // Refresh data to retrieve ID of newly added item
                LineUpItems = GetLineUpItems();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EditLineUpItem(LineUpItem item)
        {
            try
            {
                string sql = "UPDATE [LineUp] SET [Date] = @Date, [StartTime] = @StartTime, [EndTime] = @EndTime, [Stage] = @Stage, [Band] = @Band";
                sql += " WHERE [ID] = @ID";

                Database.ModifyData(sql,
                    Database.CreateParameter("@ID", item.ID),
                    Database.CreateParameter("@Date", item.Date),
                    Database.CreateParameter("@StartTime", item.StartTime),
                    Database.CreateParameter("@EndTime", item.EndTime),
                    Database.CreateParameter("@Stage", item.Stage.ID),
                    Database.CreateParameter("@Band", item.Band.ID)
                );

                LineUpItems = GetLineUpItems();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteLineUpItem(LineUpItem item)
        {
            try
            {
                string sql = "DELETE FROM [LineUp] WHERE [ID] = @ID";

                Database.ModifyData(sql,
                    Database.CreateParameter("@ID", item.ID)
                );

                LineUpItems = GetLineUpItems();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RefreshData()
        {
            LineUpItems = GetLineUpItems();
        }
    }
}
