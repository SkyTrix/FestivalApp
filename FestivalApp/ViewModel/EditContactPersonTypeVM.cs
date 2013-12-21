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
    class EditContactPersonTypeVM : ObservableObject
    {
        private bool? _dialogResult;
        public bool? DialogResult
        {
            get { return _dialogResult; }
            set { _dialogResult = value; OnPropertyChanged("DialogResult"); }
        }

        private ContactPersonType _contactPersonType;
        public ContactPersonType ContactPersonType
        {
            get { return _contactPersonType; }
            set { _contactPersonType = value; OnPropertyChanged("ContactPersonType"); }
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
                ContactPersonTypeManager.Instance.EditContactPersonType(ContactPersonType);

                // We also have to refresh the contactpersons for the changes to be visible
                ContactPersonManager.Instance.RefreshData();

                DialogResult = true;
            }
            catch (Exception)
            {
                DialogResult = false;
            }
        }
    }
}
