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
    class AddContactPersonVM : ObservableObject
    {
        private bool? _dialogResult;
        public bool? DialogResult
        {
            get { return _dialogResult; }
            set { _dialogResult = value; OnPropertyChanged("DialogResult"); }
        }

        private ContactPerson _contactPerson = new ContactPerson();
        public ContactPerson ContactPerson
        {
            get { return _contactPerson; }
            set { _contactPerson = value; OnPropertyChanged("ContactPerson"); }
        }

        private ContactPersonTypeManager _contactPersonTypeManager;
        public ContactPersonTypeManager ContactPersonTypeManager
        {
            get
            {
                if (_contactPersonTypeManager == null)
                    _contactPersonTypeManager = ContactPersonTypeManager.Instance;

                return _contactPersonTypeManager;
            }
            set { _contactPersonTypeManager = value; OnPropertyChanged("ContactPersonTypeManager"); }
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
                ContactPersonManager.Instance.AddContactPerson(ContactPerson);
                DialogResult = true;
            }
            catch (Exception)
            {
                DialogResult = false;
            }
        }
    }
}
