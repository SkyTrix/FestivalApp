using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Models
{
    public class Band
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public byte[] Picture { get; set; }

        public string PictureURL
        {
            get { return "http://localhost/bandimage/get/" + ID; }
        }

        public string Description { get; set; }

        private string _twitter;
        public string Twitter
        {
            get { return string.IsNullOrEmpty(_twitter) ? null : "@" + _twitter; }
            set { _twitter = value; }
        }

        private string _facebook;
        public string Facebook
        {
            get { return string.IsNullOrEmpty(_facebook) ? null : "http://facebook.com/" + _facebook; }
            set { _facebook = value; }
        }

        public ObservableCollection<Genre> Genres { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
