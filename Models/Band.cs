using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Models
{
    public class Band : IDataErrorInfo
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "De naam is verplicht")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 karakters, maximum 50")]
        public string Name { get; set; }

        public byte[] Picture { get; set; }

        [Required(ErrorMessage = "De omschrijving is verplicht")]
        [StringLength(255, MinimumLength = 20, ErrorMessage = "Minimum 20 karakters, maximum 255")]
        public string Description { get; set; }

        private string _twitter = string.Empty;
        [StringLength(16, MinimumLength = 0, ErrorMessage = "Een Twitter gebruikersnaam heeft maximum 15 tekens")]
        [RegularExpression(@"^[A-Za-z0-9_]+$", ErrorMessage = "De Twitter gebruikersnaam mag geen spaties of een '@' teken bevatten")]
        public string Twitter
        {
            get { return _twitter; }
            set { _twitter = value; }
        }

        private string _facebook = string.Empty;
        [StringLength(56, MinimumLength = 0, ErrorMessage = "Een Facebook gebruikersnaam heeft maximum 56 tekens")]
        [RegularExpression(@"^[a-zA-Z0-9./-]+$", ErrorMessage = "De Facebook gebruikersnaam mag geen spaties bevatten")]
        public string Facebook
        {
            get { return _facebook; }
            set { _facebook = value; }
        }

        public ObservableCollection<Genre> Genres { get; set; }

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
