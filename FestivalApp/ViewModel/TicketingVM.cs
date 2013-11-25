using FestivalApp.Model;
using FestivalApp.Model.DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalApp.ViewModel
{
    class TicketingVM : ObservableObject, IPage
    {
        private TicketTypeManager _ticketTypeManager;
        public TicketTypeManager TicketTypeManager
        {
            get
            {
                if (_ticketTypeManager == null)
                    _ticketTypeManager = TicketTypeManager.Instance;

                return _ticketTypeManager;
            }
            set { _ticketTypeManager = value; OnPropertyChanged("TicketTypeManager"); }
        }

        private TicketManager _ticketManager;
        public TicketManager TicketManager
        {
            get
            {
                if (_ticketManager == null)
                    _ticketManager = TicketManager.Instance;

                return _ticketManager;
            }
            set { _ticketManager = value; OnPropertyChanged("TicketManager"); }
        }
        

        private TicketType _selectedTicketType;
        public TicketType SelectedTicketType
        {
            get { return _selectedTicketType; }
            set { _selectedTicketType = value; OnPropertyChanged("SelectedTicketType"); }
        }

        private Ticket _selectedTicket;
        public Ticket SelectedTicket
        {
            get { return _selectedTicket; }
            set { _selectedTicket = value; OnPropertyChanged("SelectedTicket"); }
        }
        
        public string Name
        {
            get { return "Ticketing"; }
        }
    }
}
