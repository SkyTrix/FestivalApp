using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Models
{
    public class ContactPerson : IDataErrorInfo
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "De naam is verplicht")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Minimum 3 karakters, maximum 50")]
        public string Name { get; set; }

        private string _company = string.Empty;
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Maximum 50 karakters")]
        public string Company
        {
            get { return _company; }
            set { _company = value; }
        }

        [Required(ErrorMessage = "Het type is verplicht")]
        public ContactPersonType JobRole { get; set; }

        [Required(ErrorMessage = "Het adres is verplicht")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Minimum 3 karakters, maximum 50")]
        public string Address { get; set; }

        [Required(ErrorMessage = "De postcode is verplicht")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "Minimum 4 karakters, maximum 10")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "De stad is verplicht")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 karakters, maximum 50")]
        public string City { get; set; }

        [Required(ErrorMessage = "E-mail adres is verplicht is verplicht")]
        [EmailAddress(ErrorMessage = "Dit is geen geldig e-mail adres")]
        public string Email { get; set; }

        private string _phone = string.Empty;
        [RegularExpression(@"\+(9[976]\d|8[987530]\d|6[987]\d|5[90]\d|42\d|3[875]\d|
2[98654321]\d|9[8543210]|8[6421]|6[6543210]|5[87654321]|
4[987654310]|3[9643210]|2[70]|7|1)\d{1,14}$", ErrorMessage = "Telefoonnummer moet landcode bevatten (bv. +32)")]
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        private string _cellphone = string.Empty;
        [RegularExpression(@"\+(9[976]\d|8[987530]\d|6[987]\d|5[90]\d|42\d|3[875]\d|
2[98654321]\d|9[8543210]|8[6421]|6[6543210]|5[87654321]|
4[987654310]|3[9643210]|2[70]|7|1)\d{1,14}$", ErrorMessage = "Gsm nummer moet landcode bevatten (bv. +32)")]
        public string Cellphone
        {
            get { return _cellphone; }
            set { _cellphone = value; }
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
