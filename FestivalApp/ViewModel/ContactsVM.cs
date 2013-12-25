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

        private string _searchQuery = string.Empty;
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
        private bool _editingContact = false;
        private bool _deletingContact = false;

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
                if (_addingContact)
                {
                    // If the added contact doesn't conform to current filter, remove filter so we can select newly added item
                    if (!ContactPersonManager.ContactPersonConformsToFilter(ContactPersonManager.Instance.ContactPersons.Last(), SearchQuery.Trim()))
                        _searchQuery = string.Empty;
                }
                else if (_editingContact)
                {
                    // If the edited contact doesn't conform to current filter, remove filter so we can select newly added item
                    if (!ContactPersonManager.ContactPersonConformsToFilter(ContactPersonManager.Instance.ContactPersons.ToList().Find(x => x.ID == SelectedContactPerson.ID), SearchQuery.Trim()))
                        _searchQuery = string.Empty;
                }

                // Show new contact if we added one, previously selected one if we edited one, first in filter if we deleted one
                var selected =_addingContact ? ContactPersonManager.Instance.ContactPersons.Last() : SelectedContactPerson;
                UpdateFilteredContactPersons();
                SelectedContactPerson = _deletingContact ? FilteredContactPersons.FirstOrDefault() : selected;
                
                // Reflect changed filter in UI
                OnPropertyChanged("SearchQuery");
                
                // Reset booleans, actions are complete
                _addingContact = false;
                _editingContact = false;
                _deletingContact = false;

                // Force check if selected contactperson type is used and update UI
                SelectedContactPersonType = SelectedContactPersonType;
            }
        }

        private void UpdateFilteredContactPersons()
        {
            FilteredContactPersons = ContactPersonManager.GetFilteredContactPersons(SearchQuery.Trim());
            
            if(SelectedContactPerson == null && !_addingContact && !_deletingContact && !_editingContact)
                SelectedContactPerson = FilteredContactPersons.FirstOrDefault();
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
            EditContactPersonTypeVM viewModel = new EditContactPersonTypeVM();
            viewModel.ContactPersonType = SelectedContactPersonType.Copy();
            window.DataContext = viewModel;
            window.ShowDialog();
        }

        public ICommand DeleteContactPersonTypeCommand
        {
            get { return new RelayCommand(DeleteContactPersonType, CanDeleteContactPersonType); }
        }

        private bool CanDeleteContactPersonType()
        {
            return SelectedContactPersonType != null && !_selectedContactPersonTypeUsedInContactPersons;
        }

        private void DeleteContactPersonType()
        {
            try
            {
                ContactPersonTypeManager.Instance.DeleteContactPersonType(SelectedContactPersonType);
            }
            catch (Exception)
            {
            }
        }

        public ICommand AddContactPersonCommand
        {
            get { return new RelayCommand(AddContactPerson); }
        }

        private void AddContactPerson()
        {
            _addingContact = true;

            ContactPersonWindow window = new ContactPersonWindow();
            window.DataContext = new AddContactPersonVM();
            if (window.ShowDialog() == false)
            {
                _addingContact = false;
            }
        }

        public ICommand EditContactPersonCommand
        {
            get { return new RelayCommand(EditContactPerson, CanEditContactPerson); }
        }

        private bool CanEditContactPerson()
        {
            return SelectedContactPerson != null;
        }

        private void EditContactPerson()
        {
            _editingContact = true;

            ContactPersonWindow window = new ContactPersonWindow();
            EditContactPersonVM viewModel = new EditContactPersonVM();
            viewModel.ContactPerson = SelectedContactPerson.Copy();
            window.DataContext = viewModel;
            window.Title = "Contactpersoon wijzigen";
            if (window.ShowDialog() == false)
            {
                _editingContact = false;
            }
        }

        public ICommand DeleteContactPersonCommand
        {
            get { return new RelayCommand(DeleteContactPerson, CanDeleteContactPerson); }
        }

        private bool CanDeleteContactPerson()
        {
            return SelectedContactPerson != null;
        }

        private void DeleteContactPerson()
        {
            try
            {
                _deletingContact = true;

                ContactPersonManager.Instance.DeleteContactPerson(SelectedContactPerson);
            }
            catch (Exception)
            {
                _deletingContact = false;
            }
        }
    }
}
