using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class ContactPerson
    {
        private int _id;
        public int ID
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
    }
}
