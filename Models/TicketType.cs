﻿using System;
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

        [Required]
        //[DataType(DataType.Currency, ErrorMessage = "De prijs")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Het aantal is verplicht")]
        [RegularExpression(@"^([1-9][0-9]*)$", ErrorMessage = "Het aantal moet een positief getal zijn en niet 0")]
        public string AvailableTickets { get; set; }

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
