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
    public class ContactPersonTypeManager : ObservableObject
    {
         #region "Properties"
        private static readonly ContactPersonTypeManager _instance = new ContactPersonTypeManager();

        private ContactPersonTypeManager()
        {
        }

        public static ContactPersonTypeManager Instance
        {
            get
            {
                return _instance;
            }
        }

        private ObservableCollection<ContactPersonType> _contactPersonTypes;
        public ObservableCollection<ContactPersonType> ContactPersonTypes
        {
            get
            {
                if (_contactPersonTypes == null)
                    _contactPersonTypes = GetContactPersonTypes();

                return _contactPersonTypes;
            }
            set { _contactPersonTypes = value; OnPropertyChanged("ContactPersonTypes"); }
        }
        #endregion

        private ObservableCollection<ContactPersonType> GetContactPersonTypes()
        {
            try
            {
                string query = "SELECT [ID], [Name] FROM [ContactPersonTypes]";
                DbDataReader reader = Database.GetData(query);

                return GetResults(reader);
            }
            catch (Exception)
            {
                return new ObservableCollection<ContactPersonType>();
            }
        }

        public static ContactPersonType GetContactPersonTypeByID(int id)
        {
            try
            {
                string query = "SELECT [ID], [Name] FROM [ContactPersonTypes] WHERE ID = @ID";

                DbDataReader reader = Database.GetData(query,
                    Database.CreateParameter("@ID", id)
                );
                ObservableCollection<ContactPersonType> contactPersonTypes = GetResults(reader);

                return contactPersonTypes.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static ObservableCollection<ContactPersonType> GetResults(DbDataReader reader)
        {
            ObservableCollection<ContactPersonType> list = new ObservableCollection<ContactPersonType>();
            while (reader.Read())
            {
                list.Add(CreateContactPersonType(reader));
            }
            reader.Close();

            return list;
        }

        public static bool ContactPersonTypeUsedInContactPersons(ContactPersonType contactPersonType)
        {
            try
            {
                string query = "SELECT COUNT(*) AS [COUNT] FROM [ContactPersons] WHERE [JobRole] = @ID";

                DbDataReader reader = Database.GetData(query,
                    Database.CreateParameter("@ID", contactPersonType.ID)
                );

                if (reader.HasRows)
                {
                    reader.Read();
                    return (int)reader["COUNT"] > 0;
                }

                return false;
            }
            catch (Exception)
            {
                // Don't try to delete if check fails
                return true;
            }
        }

        private static ContactPersonType CreateContactPersonType(IDataRecord row)
        {
            ContactPersonType contactPersonType = new ContactPersonType();
            contactPersonType.ID = !Convert.IsDBNull(row["ID"]) ? (int)row["ID"] : -1;
            contactPersonType.Name = !Convert.IsDBNull(row["Name"]) ? row["Name"].ToString() : string.Empty;

            return contactPersonType;
        }

        public void AddContactPersonType(ContactPersonType contactPersonType)
        {
            try
            {
                string sql = "INSERT INTO [ContactPersonTypes] ([Name])";
                sql += " VALUES (@Name)";

                Database.ModifyData(sql,
                    Database.CreateParameter("@Name", contactPersonType.Name)
                );

                // Refresh data to retrieve ID of newly added item
                ContactPersonTypes = GetContactPersonTypes();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EditContactPersonType(ContactPersonType contactPersonType)
        {
            try
            {
                string sql = "UPDATE [ContactPersonTypes] SET [Name] = @Name";
                sql += " WHERE [ID] = @ID";

                Database.ModifyData(sql,
                    Database.CreateParameter("@ID", contactPersonType.ID),
                    Database.CreateParameter("@Name", contactPersonType.Name)
                );

                ContactPersonTypes = GetContactPersonTypes();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteContactPersonType(ContactPersonType contactPersonType)
        {
            try
            {
                string sql = "DELETE FROM [ContactPersonTypes] WHERE [ID] = @ID";

                Database.ModifyData(sql,
                    Database.CreateParameter("@ID", contactPersonType.ID)
                );

                ContactPersonTypes = GetContactPersonTypes();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
