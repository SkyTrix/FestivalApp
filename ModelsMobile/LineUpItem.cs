using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class LineUpItem
    {
        public int ID { get; set; }

        public DateTime Date { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public Stage Stage { get; set; }

        public Band Band { get; set; }
    }
}
