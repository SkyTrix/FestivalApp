using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalApp.Model
{
    class ContactPerson
    {
        private string _id;
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _company;
        public string Company
        {
            get { return _company; }
            set { _company = value; }
        }

        private ContactPersonType _jobRole;
        public ContactPersonType JobRole
        {
            get { return _jobRole; }
            set { _jobRole = value; }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        private string _postalCode;
        public string PostalCode
        {
            get { return _postalCode; }
            set { _postalCode = value; }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        private string _cellphone;
        public string Cellphone
        {
            get { return _cellphone; }
            set { _cellphone = value; }
        }

        public static ObservableCollection<ContactPerson> GetContactPersons()
        {
            ObservableCollection<ContactPerson> list = new ObservableCollection<ContactPerson>();
            using (StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "ContactPersons.csv"))
            {
                string line = sr.ReadLine();
                if (line != null)
                    line = sr.ReadLine();

                while (line != null)
                {
                    ContactPerson contactPerson = CreateContactPerson(line);
                    if (contactPerson != null)
                        list.Add(contactPerson);

                    line = sr.ReadLine();
                }

                return list;
            }
        }

        private static ContactPerson CreateContactPerson(string line)
        {
            string[] elements = line.Split(';');
            if (elements.Length != 10)
                return null;

            ContactPersonType type = ContactPersonType.GetContactPersonTypeByID(elements[3]);
            ContactPerson contactPerson = new ContactPerson { ID = elements[0], Name = elements[1], Company = elements[2], JobRole = type, Address = elements[4], PostalCode = elements[5], City = elements[6], Email = elements[7], Phone = elements[8], Cellphone = elements[9] };

            return contactPerson;
        }

        public static ObservableCollection<ContactPerson> GetFilteredContactPersons(ObservableCollection<ContactPerson> contactPersons, string filter)
        {
            if (filter == null || filter.Equals(""))
                return new ObservableCollection<ContactPerson>(contactPersons);

            List<ContactPerson> filteredContactPersons = contactPersons.ToList().FindAll(x => x.Name.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) >= 0);
            ObservableCollection<ContactPerson> result = new ObservableCollection<ContactPerson>(filteredContactPersons);

            return result;
        }

        public static void AddContactPerson(ContactPerson contactPerson)
        {
            Console.WriteLine("Adding contactPerson");
        }

        public static void EditContactPerson(ContactPerson contactPerson)
        {
            Console.WriteLine("Editing contactPerson");
        }

        public static void DeleteContactPerson(ContactPerson contactPerson)
        {
            Console.WriteLine("Deleting contactPerson");
        }
    }
}
