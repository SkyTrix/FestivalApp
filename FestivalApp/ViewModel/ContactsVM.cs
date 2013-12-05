using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalApp.ViewModel
{
    class ContactsVM : ObservableObject, IPage
    {
        private ObservableCollection<ContactPerson> _contactPersons;
        public ObservableCollection<ContactPerson> ContactPersons
        {
            get
            {
                if (_contactPersons == null)
                {
                    //_contactPersons = ContactPerson.GetContactPersons();
                }

                return _contactPersons;
            }
            set
            {
                _contactPersons = value;
                OnPropertyChanged("ContactPersons");

                //FilteredContactPersons = ContactPerson.GetFilteredContactPersons(_contactPersons, SearchQuery);
            }
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

                //FilteredContactPersons = ContactPerson.GetFilteredContactPersons(ContactPersons, _searchQuery);
            }
        }

        private ObservableCollection<ContactPerson> _filteredContactPersons;
        public ObservableCollection<ContactPerson> FilteredContactPersons
        {
            get
            {
                if (_filteredContactPersons == null)
                {
                    //ContactPersons = ContactPerson.GetContactPersons();
                }

                return _filteredContactPersons;
            }
            set { _filteredContactPersons = value; OnPropertyChanged("FilteredContactPersons"); }
        }
        

        //private ObservableCollection<ContactPersonType> _contactPersonTypes;
        //public ObservableCollection<ContactPersonType> ContactPersonTypes
        //{
        //    get
        //    {
        //        if (_contactPersonTypes == null)
        //            _contactPersonTypes = ContactPersonType.GetContactPersonTypes();

        //        return _contactPersonTypes;
        //    }
        //    set { _contactPersonTypes = value; OnPropertyChanged("ContactPersonTypes"); }
        //}

        private ContactPersonType _selectedContactPersonType;
        public ContactPersonType SelectedContactPersonType
        {
            get { return _selectedContactPersonType; }
            set { _selectedContactPersonType = value; OnPropertyChanged("SelectedContactPersonType"); }
        }

        public string Name
        {
            get { return "Contactpersonen"; }
        }
    }
}
