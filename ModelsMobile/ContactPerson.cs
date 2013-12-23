using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class ContactPerson
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Company { get; set; }

        public ContactPersonType JobRole { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Cellphone { get; set; }
    }
}
