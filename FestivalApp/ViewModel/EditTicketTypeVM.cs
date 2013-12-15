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
    class EditTicketTypeVM : ObservableObject
    {
        private bool? _dialogResult;
        public bool? DialogResult
        {
            get { return _dialogResult; }
            set { _dialogResult = value; OnPropertyChanged("DialogResult"); }
        }

        private TicketType _ticketType = new TicketType();
        public TicketType TicketType
        {
            get { return _ticketType; }
            set { _ticketType = value; OnPropertyChanged("TicketType"); }
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
                TicketTypeManager.Instance.EditTicketType(TicketType);
                TicketManager.Instance.RefreshData();

                DialogResult = true;
            }
            catch (Exception)
            {
            }
        }
    }
}
