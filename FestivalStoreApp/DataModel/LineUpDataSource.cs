using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FestivalStoreApp.DataModel
{
    public class LineUpDataGroup
    {
        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return _uniqueId; }
            set { _uniqueId = value; }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private ObservableCollection<LineUpItem> _items = new ObservableCollection<LineUpItem>();
        public ObservableCollection<LineUpItem> Items
        {
            get { return this._items; }
        }

        public IEnumerable<LineUpItem> TopItems
        {
            get { return this._items.Take(12); }
        }

        public int LineUpItemsCount
        {
            get
            {
                return this.Items.Count;
            }
        }
    }

    public sealed class LineUpDataSource
    {
        //public event EventHandler RecipesLoaded;

        private static LineUpDataSource _lineUpDataSource = new LineUpDataSource();

        private ObservableCollection<LineUpDataGroup> _allGroups = new ObservableCollection<LineUpDataGroup>();
        public ObservableCollection<LineUpDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<LineUpDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");

            return _lineUpDataSource.AllGroups;
        }

        public static LineUpDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _lineUpDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static LineUpItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _lineUpDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.ID.ToString().Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static async Task LoadRemoteDataAsync()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new
                System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = await client.GetAsync("http://localhost/api/lineup");
                if (response.IsSuccessStatusCode)
                {
                    Stream stream = await response.Content.ReadAsStreamAsync();
                    DataContractSerializer dxml = new DataContractSerializer(typeof(List<LineUpItem>));
                    List<LineUpItem> list = dxml.ReadObject(stream) as List<LineUpItem>;

                    // Convert lineup items into groups of lineupitems
                    foreach (LineUpItem item in list)
                    {
                        LineUpDataGroup group = null;

                        group = _lineUpDataSource.AllGroups.FirstOrDefault(c => c.UniqueId.Equals(item.Stage.ID.ToString()));

                        if (group == null)
                        {
                            group = new LineUpDataGroup() { UniqueId = item.Stage.ID.ToString(), Title = item.Stage.Name };
                            _lineUpDataSource.AllGroups.Add(group);
                        }

                        group.Items.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }
    }
}
