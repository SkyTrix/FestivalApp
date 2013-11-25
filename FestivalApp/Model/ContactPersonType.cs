using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalApp.Model
{
    class ContactPersonType
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

        public static ObservableCollection<ContactPersonType> GetContactPersonTypes()
        {
            ObservableCollection<ContactPersonType> list = new ObservableCollection<ContactPersonType>();
            using (StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "ContactPersonTypes.csv"))
            {
                string line = sr.ReadLine();
                if (line != null)
                    line = sr.ReadLine();

                while (line != null)
                {
                    ContactPersonType contactPersonType = CreateContactPersonType(line);
                    if (contactPersonType != null)
                        list.Add(contactPersonType);

                    line = sr.ReadLine();
                }

                return list;
            }
        }

        private static ContactPersonType CreateContactPersonType(string line)
        {
            string[] elements = line.Split(';');
            if (elements.Length != 2)
                return null;

            ContactPersonType contactPersonType = new ContactPersonType { ID = elements[0], Name = elements[1] };

            return contactPersonType;
        }

        public static ContactPersonType GetContactPersonTypeByID(string id)
        {
            // TODO: change to db query
            return GetContactPersonTypes().ToList().Find(x => x.ID == id);
        }

        public static void AddContactPersonType(ContactPersonType contactPersonType)
        {
            Console.WriteLine("Adding contactPersonType");
        }

        public static void EditContactPersonType(ContactPersonType contactPersonType)
        {
            Console.WriteLine("Editing contactPersonType");
        }

        public static void DeleteContactPersonType(ContactPersonType contactPersonType)
        {
            Console.WriteLine("Deleting contactPersonType");
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
