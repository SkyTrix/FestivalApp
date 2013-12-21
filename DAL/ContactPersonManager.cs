using FestivalApp.Utilities;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ContactPersonManager : ObservableObject
    {
        #region "Properties"
        private static readonly ContactPersonManager _instance = new ContactPersonManager();

        private ContactPersonManager()
        {
        }

        public static ContactPersonManager Instance
        {
            get
            {
                return _instance;
            }
        }

        private ObservableCollection<ContactPerson> _contactPersons;
        public ObservableCollection<ContactPerson> ContactPersons
        {
            get
            {
                if (_contactPersons == null)
                    _contactPersons = GetContactPersons();

                return _contactPersons;
            }
            set { _contactPersons = value; OnPropertyChanged("ContactPersons"); }
        }
        #endregion

        private ObservableCollection<ContactPerson> GetContactPersons()
        {
            try
            {
                string query = "SELECT [ID], [Name], [Company], [JobRole], [Address], [PostalCode], [City], [Email], [Phone], [CellPhone] FROM [ContactPersons]";
                DbDataReader reader = Database.GetData(query);

                return GetResults(reader);
            }
            catch (Exception)
            {
                return new ObservableCollection<ContactPerson>();
            }
        }

        private static ObservableCollection<ContactPerson> GetResults(DbDataReader reader)
        {
            ObservableCollection<ContactPerson> list = new ObservableCollection<ContactPerson>();
            while (reader.Read())
            {
                list.Add(CreateContactPerson(reader));
            }
            reader.Close();

            return list;
        }

        private static ContactPerson CreateContactPerson(IDataRecord row)
        {
            ContactPerson contactPerson = new ContactPerson();
            contactPerson.ID = !Convert.IsDBNull(row["ID"]) ? (int)row["ID"] : -1;
            contactPerson.Name = !Convert.IsDBNull(row["Name"]) ? row["Name"].ToString() : string.Empty;
            contactPerson.Company = !Convert.IsDBNull(row["Company"]) ? row["Company"].ToString() : string.Empty;
            contactPerson.Address = !Convert.IsDBNull(row["Address"]) ? row["Address"].ToString() : string.Empty;
            contactPerson.PostalCode = !Convert.IsDBNull(row["PostalCode"]) ? row["PostalCode"].ToString() : string.Empty;
            contactPerson.City = !Convert.IsDBNull(row["City"]) ? row["City"].ToString() : string.Empty;
            contactPerson.Email = !Convert.IsDBNull(row["Email"]) ? row["Email"].ToString() : string.Empty;
            contactPerson.Phone = !Convert.IsDBNull(row["Phone"]) ? row["Phone"].ToString() : string.Empty;
            contactPerson.Cellphone = !Convert.IsDBNull(row["Cellphone"]) ? row["Cellphone"].ToString() : string.Empty;

            contactPerson.JobRole = !Convert.IsDBNull(row["JobRole"]) ? ContactPersonTypeManager.GetContactPersonTypeByID((int)row["JobRole"]) : null;

            return contactPerson;
        }

        public void AddContactPerson(ContactPerson contactPerson)
        {
            try
            {
                string sql = "INSERT INTO [ContactPersons] ([Name], [Company], [JobRole], [Address], [PostalCode], [City], [Email], [Phone], [Cellphone])";
                sql += " VALUES (@Name, @Company, @JobRole, @Address, @PostalCode, @City, @Email, @Phone, @Cellphone)";

                Database.ModifyData(sql,
                    Database.CreateParameter("@Name", contactPerson.Name),
                    Database.CreateParameter("@Company", contactPerson.Company),
                    Database.CreateParameter("@JobRole", contactPerson.JobRole.ID),
                    Database.CreateParameter("@Address", contactPerson.Address),
                    Database.CreateParameter("@PostalCode", contactPerson.PostalCode),
                    Database.CreateParameter("@City", contactPerson.City),
                    Database.CreateParameter("@Email", contactPerson.Email),
                    Database.CreateParameter("@Phone", contactPerson.Phone),
                    Database.CreateParameter("@Cellphone", contactPerson.Cellphone)
                );

                ContactPersons = GetContactPersons();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EditContactPerson(ContactPerson contactPerson)
        {
            try
            {
                string sql = "UPDATE [ContactPersons] SET [Name] = @Name, [Company] = @Company, [JobRole] = @JobRole, [Address] = @Address, [PostalCode] = @PostalCode, [City] = @City, [Email] = @Email, [Phone] = @Phone, [Cellphone] = @Cellphone";
                sql += " WHERE [ID] = @ID";

                Database.ModifyData(sql,
                    Database.CreateParameter("@ID", contactPerson.ID),
                    Database.CreateParameter("@Name", contactPerson.Name),
                    Database.CreateParameter("@Company", contactPerson.Company),
                    Database.CreateParameter("@JobRole", contactPerson.JobRole.ID),
                    Database.CreateParameter("@Address", contactPerson.Address),
                    Database.CreateParameter("@PostalCode", contactPerson.PostalCode),
                    Database.CreateParameter("@City", contactPerson.City),
                    Database.CreateParameter("@Email", contactPerson.Email),
                    Database.CreateParameter("@Phone", contactPerson.Phone),
                    Database.CreateParameter("@Cellphone", contactPerson.Cellphone)
                );

                ContactPersons = GetContactPersons();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteContactPerson(ContactPerson contactPerson)
        {
            try
            {
                string sql = "DELETE FROM [ContactPersons] WHERE [ID] = @ID";

                Database.ModifyData(sql,
                    Database.CreateParameter("@ID", contactPerson.ID)
                );

                ContactPersons = GetContactPersons();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static ObservableCollection<ContactPerson> GetFilteredContactPersons(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return new ObservableCollection<ContactPerson>(Instance.ContactPersons);

            List<ContactPerson> filteredContactPersons = Instance.ContactPersons.ToList().FindAll(x => x.Name.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) >= 0);

            return new ObservableCollection<ContactPerson>(filteredContactPersons); ;
        }

        public void RefreshData()
        {
            ContactPersons = GetContactPersons();
        }
    }
}
