using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FestivalWebsite.ViewModels
{
    public class ReservationsOverviewVM
    {
        public IEnumerable<Ticket> Tickets { get; set; }
        public IEnumerable<TicketTypeVM> TicketTypes { get; set; }
        public double TotalRevenue { get; set; }
    }
}