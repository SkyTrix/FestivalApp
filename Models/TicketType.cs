using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Models
{
    public class TicketType : IDataErrorInfo
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "De naam is verplicht")]
        [StringLength(50, ErrorMessage = "Maximum 50 karakters")]
        public string Name { get; set; }

        private double _price;
        public double Price
        {
            get { return _price; }
            set { _price = value; _priceString = PriceToPriceString(_price); }
        }
        private string _priceString;
        [Required(ErrorMessage = "De prijs is verplicht")]
        [RegularExpression(@"^\d+((,|.)\d{1,2})?$", ErrorMessage = "Dit is geen correct prijs formaat (x of x,xx of x.xx)")]
        public string PriceString
        {
            get { return _priceString; }
            set { _priceString = value; _price = PriceStringToDouble(_priceString); }
        }

        [Required(ErrorMessage = "Het aantal is verplicht")]
        [RegularExpression(@"^([1-9][0-9]*)$", ErrorMessage = "Het aantal moet een positief getal zijn en niet 0")]
        public string AvailableTickets { get; set; }

        public double PriceStringToDouble(string price)
        {
            price = price.Replace('.', ',');

            double p;
            double.TryParse(price, out p);

            return p;
        }

        public string PriceToPriceString(double price)
        {
            return price.ToString();
        }

        public override string ToString()
        {
            return this.Name;
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
