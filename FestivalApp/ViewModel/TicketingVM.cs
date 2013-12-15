using DAL;
using FestivalApp.View;
using GalaSoft.MvvmLight.Command;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        public ICommand AddTicketTypeCommand
        {
            get { return new RelayCommand(AddTicketType); }
        }

        private void AddTicketType()
        {
            TicketTypeWindow window = new TicketTypeWindow();
            window.DataContext = new AddTicketTypeVM();
            if (window.ShowDialog() == true)
            {
                // Force reload of ticket type UI
                TicketTypes = null;
            }
        }

        public ICommand EditTicketTypeCommand
        {
            get { return new RelayCommand(EditTicketType, CanEditTicketType); }
        }

        private bool CanEditTicketType()
        {
            return SelectedTicketTypeVM != null && SelectedTicketTypeVM.TicketType != null;
        }

        private void EditTicketType()
        {
            TicketTypeWindow window = new TicketTypeWindow();
            EditTicketTypeVM viewModel = new EditTicketTypeVM();
            viewModel.TicketType = SelectedTicketTypeVM.TicketType.Copy();
            window.DataContext = viewModel;
            window.Title = "Ticket type wijzigen";
            if (window.ShowDialog() == true)
            {
                // Force reload of ticket type UI
                TicketTypes = null;
            }
        }

        public ICommand AddReservationCommand
        {
            get { return new RelayCommand(AddReservation); }
        }

        private void AddReservation()
        {
            ReservationWindow window = new ReservationWindow();
            window.DataContext = new AddReservationVM();
            if (window.ShowDialog() == true)
            {
                // Force reload of ticket type UI
                TicketTypes = null;
            }
        }

        public ICommand EditReservationCommand
        {
            get { return new RelayCommand(EditReservation, CanEditReservation); }
        }

        private bool CanEditReservation()
        {
            return SelectedTicket != null;
        }

        private void EditReservation()
        {
            ReservationWindow window = new ReservationWindow();
            EditReservationVM viewModel = new EditReservationVM();
            viewModel.Ticket = SelectedTicket.Copy();
            window.DataContext = viewModel;
            window.Title = "Reservatie wijzigen";
            if (window.ShowDialog() == true)
            {
                // Force reload of ticket type UI
                TicketTypes = null;
            }
        }
    }
}
