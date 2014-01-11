using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FestivalWebsite.ViewModels
{
    public class TicketTypeAmountVM
    {
        public TicketType TicketType { get; set; }
        public int RemainingTickets { get; set; }

        [Required(ErrorMessage = "Please enter an amount")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please enter a positive number")]
        public int Amount { get; set; }
    }
}