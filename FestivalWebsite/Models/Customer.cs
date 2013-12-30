using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FestivalWebsite.Models
{
    public class Customer
    {
        [Required(ErrorMessage = "Please enter your first name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Must be at least 2 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Must be at least 2 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
    }
}