using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Models
{
    public class LineUpItem : IDataErrorInfo
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "De datum is verplicht")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "De starttijd is verplicht")]
        [RegularExpression(@"([01][0-9]|2[0-3]):[0-5][0-9]", ErrorMessage = "Tijdstip moet voldoen aan het formaat 'HH:MM'")]
        public string StartTime { get; set; }

        [Required(ErrorMessage = "De eindtijd is verplicht")]
        [RegularExpression(@"([01][0-9]|2[0-3]):[0-5][0-9]", ErrorMessage = "Tijdstip moet voldoen aan het formaat 'HH:MM'")]
        public string EndTime { get; set; }

        [Required(ErrorMessage = "De stage is verplicht")]
        public Stage Stage { get; set; }

        [Required(ErrorMessage = "De band is verplicht")]
        public Band Band { get; set; }

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
