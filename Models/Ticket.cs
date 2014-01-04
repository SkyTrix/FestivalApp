using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Models
{
    public class Ticket : IDataErrorInfo
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "De naam is verplicht")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Minimum 3 karakters, maximum 50")]
        public string TicketHolder { get; set; }

        [Required(ErrorMessage = "Het e-mail adres is verplicht")]
        [EmailAddress(ErrorMessage = "Dit is geen geldig e-mail adres")]
        public string TicketHolderEmail { get; set; }

        [Required(ErrorMessage = "Het ticket type is verplicht")]
        public TicketType TicketType { get; set; }

        [Required(ErrorMessage = "Het aantal is verplicht")]
        [RegularExpression(@"^([1-9][0-9]*)$", ErrorMessage = "Het aantal moet een positief getal zijn en niet 0")]
        public string Amount { get; set; }

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
