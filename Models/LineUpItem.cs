using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class LineUpItem
    {
        private string _id;
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private string _startTime;
        public string StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        private string _endTime;
        public string EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        private Stage _stage;
        public Stage Stage
        {
            get { return _stage; }
            set { _stage = value; }
        }

        private Band _band;
        public Band Band
        {
            get { return _band; }
            set { _band = value; }
        }
    }
}
