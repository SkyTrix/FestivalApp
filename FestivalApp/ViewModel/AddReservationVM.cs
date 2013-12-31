using DAL;
using GalaSoft.MvvmLight.Command;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FestivalApp.ViewModel
{
    class AddReservationVM : ObservableObject
    {
        private bool? _dialogResult;
        public bool? DialogResult
        {
            get { return _dialogResult; }
            set { _dialogResult = value; OnPropertyChanged("DialogResult"); }
        }

        private Ticket _ticket = new Ticket();
        public Ticket Ticket
        {
            get { return _ticket; }
            set { _ticket = value; OnPropertyChanged("Ticket"); }
        }

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

        private string _reservationError;
        public string ReservationError
        {
            get { return _reservationError; }
            set { _reservationError = value; OnPropertyChanged("ReservationError"); }
        }

        public ICommand CancelCommand
        {
            get { return new RelayCommand(Cancel); }
        }

        public ICommand SaveCommand
        {
            get { return new RelayCommand(Save, Ticket.IsValid); }
        }

        private void Cancel()
        {
            DialogResult = false;
        }

        private void Save()
        {
            // Check if more tickets are reserved than there are available
            if (int.Parse(Ticket.Amount) > TicketTypeManager.CountTicketsRemainingForTicketType(Ticket.TicketType.ID))
            {
                ReservationError = "Fout bij toevoegen: er kunnen niet meer tickets gereserveerd worden dan er nog beschikbaar zijn.";
                return;
            }

            // Remove possible error message
            ReservationError = null;

            try
            {
                TicketManager.Instance.AddTicket(Ticket);

                DialogResult = true;
            }
            catch (Exception)
            {
                DialogResult = false;
            }
        }
    }
}
