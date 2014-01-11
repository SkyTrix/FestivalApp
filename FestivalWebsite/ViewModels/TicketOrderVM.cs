using FestivalWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FestivalWebsite.ViewModels
{
    public class TicketOrderVM
    {
        public IList<TicketTypeAmountVM> TicketTypes { get; set; }
        public Customer Customer { get; set; }
    }
}