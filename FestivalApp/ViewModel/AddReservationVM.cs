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

        public ICommand CancelCommand
        {
            get { return new RelayCommand(Cancel); }
        }

        public ICommand SaveCommand
        {
            get { return new RelayCommand(Save); }
        }

        private void Cancel()
        {
            DialogResult = false;
        }

        private void Save()
        {
            try
            {
                TicketManager.Instance.AddTicket(Ticket);

                DialogResult = true;
            }
            catch (Exception)
            {
            }
        }
    }
}
