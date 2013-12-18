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
    class ContactsVM : ObservableObject, IPage
    {
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

        private ContactPersonType _selectedContactPersonType;
        public ContactPersonType SelectedContactPersonType
        {
            get { return _selectedContactPersonType; }
            set
            {
                _selectedContactPersonType = value;
                _selectedContactPersonTypeUsedInContactPersons = false;

                if (_selectedContactPersonType != null)
                {
                    _selectedContactPersonTypeUsedInContactPersons = ContactPersonTypeManager.ContactPersonTypeUsedInContactPersons(_selectedContactPersonType);
                }

                OnPropertyChanged("SelectedContactPersonType");
            }
        }

        private bool _selectedContactPersonTypeUsedInContactPersons = false;

        private ObservableCollection<ContactPerson> _filteredContactPersons;
        public ObservableCollection<ContactPerson> FilteredContactPersons
        {
            get
            {
                if (_filteredContactPersons == null)
                {
                    UpdateFilteredContactPersons();
                }

                return _filteredContactPersons;
            }
            set { _filteredContactPersons = value; OnPropertyChanged("FilteredContactPersons"); }
        }

        private ContactPerson _selectedContactPerson;
        public ContactPerson SelectedContactPerson
        {
            get { return _selectedContactPerson; }
            set { _selectedContactPerson = value; OnPropertyChanged("SelectedContactPerson"); }
        }

        private string _searchQuery;
        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                _searchQuery = value;
                OnPropertyChanged("SearchQuery");

                UpdateFilteredContactPersons();
            }
        }

        private string _contactPersonType = string.Empty;
        public string ContactPersonType
        {
            get { return _contactPersonType; }
            set { _contactPersonType = value; OnPropertyChanged("ContactPersonType"); }
        }

        public string Name
        {
            get { return "Contactpersonen"; }
        }

        private bool _addingContact = false;

        public ContactsVM()
        {
            try
            {
                SelectedContactPerson = FilteredContactPersons.FirstOrDefault();

                ContactPersonManager.Instance.PropertyChanged += Instance_PropertyChanged;
            }
            catch (Exception)
            {
            }
        }

        void Instance_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ContactPersons"))
            {
                // Show new contact if we added one, previously selected one if we didn't add one
                var selected = _addingContact ? ContactPersonManager.Instance.ContactPersons.Last() : SelectedContactPerson;
                UpdateFilteredContactPersons();
                SelectedContactPerson = selected;
                _addingContact = false;
            }
        }

        private void UpdateFilteredContactPersons()
        {
            FilteredContactPersons = ContactPersonManager.GetFilteredContactPersons(SearchQuery);
        }

        public ICommand AddContactPersonTypeCommand
        {
            get { return new RelayCommand(AddContactPersonType, CanAddContactPersonType); }
        }

        private bool CanAddContactPersonType()
        {
            return ContactPersonType != null && ContactPersonType.Length > 2;
        }

        private void AddContactPersonType()
        {
            try
            {
                ContactPersonType contactPersonType = new ContactPersonType();
                contactPersonType.Name = ContactPersonType;
                ContactPersonTypeManager.Instance.AddContactPersonType(contactPersonType);
                ContactPersonType = string.Empty;
            }
            catch (Exception)
            { 
            }
        }

        public ICommand EditContactPersonTypeCommand
        {
            get { return new RelayCommand(EditContactPersonType, CanEditContactPersonType); }
        }

        private bool CanEditContactPersonType()
        {
            return SelectedContactPersonType != null;
        }

        private void EditContactPersonType()
        {
            EditContactPersonTypeWindow window = new EditContactPersonTypeWindow();
            ((EditContactPersonTypeVM)window.DataContext).ContactPersonType = SelectedContactPersonType.Copy();
            window.ShowDialog();
        }

        public ICommand DeleteContactPersonTypeCommand
        {
            get { return new RelayCommand(DeleteContactPersonType, CanDeleteContactPersonType); }
        }

        private bool CanDeleteContactPersonType()
        {
            return SelectedContactPersonType != null && !_selectedContactPersonTypeUsedInContactPersons; // fix
        }

        private void DeleteContactPersonType()
        {
            ContactPersonTypeManager.Instance.DeleteContactPersonType(SelectedContactPersonType);
        }
    }
}
