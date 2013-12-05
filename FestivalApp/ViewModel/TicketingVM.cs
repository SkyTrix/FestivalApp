using DAL;
using Models;
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

        private ObservableCollection<TicketTypeVM> _ticketTypes;
        public ObservableCollection<TicketTypeVM> TicketTypes
        {
            get
            {
                if (_ticketTypes == null)
                {
                    // Use a presentation model so we can display both the ticket types and the tickets remaining for the type
                    ObservableCollection<TicketTypeVM> ticketTypes = new ObservableCollection<TicketTypeVM>();

                    foreach (TicketType ticketType in TicketTypeManager.Instance.TicketTypes)
                    {
                        ticketTypes.Add(new TicketTypeVM()
                        {
                            TicketType = ticketType,
                            RemainingTickets = TicketTypeManager.CountTicketsRemainingForTicketType(ticketType.ID)
                        });
                    }

                    _ticketTypes = ticketTypes;
                }

                return _ticketTypes;
            }
            set { _ticketTypes = value; OnPropertyChanged("TicketTypes"); }
        }

        private TicketTypeVM _selectedTicketTypeVM;
        public TicketTypeVM SelectedTicketTypeVM
        {
            get { return _selectedTicketTypeVM; }
            set { _selectedTicketTypeVM = value; OnPropertyChanged("SelectedTicketTypeVM"); }
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
