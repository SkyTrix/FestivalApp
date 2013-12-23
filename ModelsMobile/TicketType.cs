using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class TicketType
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int AvailableTickets { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
