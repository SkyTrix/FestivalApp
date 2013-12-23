using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Ticket
    {
        public int ID { get; set; }

        public string TicketHolder { get; set; }

        public string TicketHolderEmail { get; set; }

        public TicketType TicketType { get; set; }

        public int Amount { get; set; }
    }
}
