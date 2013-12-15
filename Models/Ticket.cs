﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Ticket
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _ticketHolder;
        public string TicketHolder
        {
            get { return _ticketHolder; }
            set { _ticketHolder = value; }
        }

        private string _ticketHolderEmail;
        public string TicketHolderEmail
        {
            get { return _ticketHolderEmail; }
            set { _ticketHolderEmail = value; }
        }

        private TicketType _ticketType;
        public TicketType TicketType
        {
            get { return _ticketType; }
            set { _ticketType = value; }
        }

        private int _amount;
        public int Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }
    }
}
