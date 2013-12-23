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

        public string Description { get; set; }

        public string Twitter { get; set; }

        public string Facebook { get; set; }

        public ObservableCollection<Genre> Genres { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
