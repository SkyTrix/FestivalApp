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
    public class NewsItemManager
    {
        public static List<NewsItem> GetNewsItems()
        {
            try
            {
                string query = "SELECT [ID], [Title], [Content] FROM [News]";
                DbDataReader reader = Database.GetData(query);

                return GetResults(reader);
            }
            catch (Exception)
            {
                return new List<NewsItem>();
            }
        }

        private static List<NewsItem> GetResults(DbDataReader reader)
        {
            List<NewsItem> list = new List<NewsItem>();
            while (reader.Read())
            {
                list.Add(CreateNewsItem(reader));
            }
            reader.Close();

            return list;
        }

        private static NewsItem CreateNewsItem(IDataRecord row)
        {
            NewsItem newsItem = new NewsItem();
            newsItem.ID = !Convert.IsDBNull(row["ID"]) ? (int)row["ID"] : -1;
            newsItem.Title = !Convert.IsDBNull(row["Title"]) ? row["Title"].ToString() : string.Empty;
            newsItem.Content = !Convert.IsDBNull(row["Content"]) ? row["Content"].ToString() : string.Empty;

            return newsItem;
        }
    }
}
