using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class TicketType
    {
        private string _id;
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private double _price;
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private int _availableTickets;
        public int AvailableTickets
        {
            get { return _availableTickets; }
            set { _availableTickets = value; }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
