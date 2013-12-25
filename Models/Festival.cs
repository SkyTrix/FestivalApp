using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Models
{
    public class Festival : IDataErrorInfo
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        [Required(ErrorMessage = "De naam is verplicht")]
        [StringLength(50, ErrorMessage = "Maximum 50 karakters")]
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

        public string Error
        {
            get { return "Het object is niet valid"; }
        }

        public string this[string columnName]
        {
            get
            {
                try
                {
                    object value = this.GetType().GetProperty(columnName).GetValue(this);
                    Validator.ValidateProperty(value, new ValidationContext(this, null, null)
                    {
                        MemberName = columnName
                    });
                }
                catch (ValidationException ex)
                {
                    return ex.Message;
                }
                return String.Empty;
            }
        }

        public bool IsValid()
        {
            return Validator.TryValidateObject(this, new ValidationContext(this, null, null), null, true);
        }
    }
}
