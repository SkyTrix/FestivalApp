using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Models
{
    public class Festival
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; UpdateFestivalDates(); }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; UpdateFestivalDates(); }
        }

        private ObservableCollection<DateTime> _festivalDates;
        public ObservableCollection<DateTime> FestivalDates
        {
            get { return _festivalDates; }
            private set { _festivalDates = value; }
        }

        private void UpdateFestivalDates()
        {
            ObservableCollection<DateTime> dates = new ObservableCollection<DateTime>();

            if (StartDate != null && EndDate != null)
            {
                for (DateTime date = StartDate; date <= EndDate; date = date.AddDays(1))
                {
                    dates.Add(date);
                }

                FestivalDates = dates;
            }
        }
    }
}
