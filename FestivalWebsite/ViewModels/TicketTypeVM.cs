using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FestivalWebsite.ViewModels
{
    public class TicketTypeVM
    {
        public TicketType TicketType { get; set; }
        public int RemainingTickets { get; set; }
        public int SoldTickets
        {
            get
            {
                return int.Parse(TicketType.AvailableTickets) - RemainingTickets;
            }
        }
        public double Revenue
        {
            get
            {
                return SoldTickets * TicketType.Price;
            }
        }
    }
}