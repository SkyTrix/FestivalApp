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
    public class TicketTypeManager : ObservableObject
    {
        #region "Properties"
        private static readonly TicketTypeManager _instance = new TicketTypeManager();

        private TicketTypeManager()
        {
        }

        public static TicketTypeManager Instance
        {
            get
            {
                return _instance;
            }
        }

        private ObservableCollection<TicketType> _ticketTypes;
        public ObservableCollection<TicketType> TicketTypes
        {
            get
            {
                if (_ticketTypes == null)
                    _ticketTypes = GetTicketTypes();

                return _ticketTypes;
            }
            set { _ticketTypes = value; OnPropertyChanged("TicketTypes"); }
        }   
        #endregion

        private ObservableCollection<TicketType> GetTicketTypes()
        {
            try
            {
                string query = "SELECT [ID], [Name], [Price], [AvailableTickets] FROM [TicketTypes]";
                DbDataReader reader = Database.GetData(query);

                return GetResults(reader);
            }
            catch (Exception)
            {
                return new ObservableCollection<TicketType>();
            }
        }

        public static TicketType GetTicketTypeByID(string id)
        {
            try
            {
                string query = "SELECT [ID], [Name], [Price], [AvailableTickets] FROM [TicketTypes] WHERE ID = @ID";
                DbParameter idPar = Database.CreateParameter("@ID", id);

                DbDataReader reader = Database.GetData(query, idPar);

                ObservableCollection<TicketType> ticketTypes = GetResults(reader);
                if (ticketTypes.Count > 0)
                    return ticketTypes[0];

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static int CountTicketsRemainingForTicketType(int id)
        {
            try
            {
                string query = "SELECT ([AvailableTickets] - ISNULL((SELECT SUM([amount]) FROM [Tickets] WHERE [Type] = [TicketTypes].[ID]), 0)) AS [RemainingTickets] FROM [TicketTypes] WHERE ID = @ID";
                DbParameter idPar = Database.CreateParameter("@ID", id);
                DbDataReader reader = Database.GetData(query, idPar);

                if (reader.HasRows)
                {
                    reader.Read();
                    return (int)reader["RemainingTickets"];
                }

                return 0;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private static ObservableCollection<TicketType> GetResults(DbDataReader reader)
        {
            ObservableCollection<TicketType> list = new ObservableCollection<TicketType>();
            while (reader.Read())
            {
                list.Add(CreateTicketType(reader));
            }
            reader.Close();

            return list;
        }

        private static TicketType CreateTicketType(IDataRecord row)
        {
            TicketType ticketType = new TicketType();
            ticketType.ID = !Convert.IsDBNull(row["ID"]) ? (int)row["ID"] : -1;
            ticketType.Name = !Convert.IsDBNull(row["Name"]) ? row["Name"].ToString() : string.Empty;
            ticketType.Price = !Convert.IsDBNull(row["Price"]) ? Convert.ToDouble(row["Price"]) : 0;
            ticketType.AvailableTickets = !Convert.IsDBNull(row["AvailableTickets"]) ? (int)row["AvailableTickets"] : 0;

            return ticketType;
        }

        public void AddTicketType(TicketType ticketType)
        {
            try
            {
                string sql = "INSERT INTO [TicketTypes] ([Name], [Price], [AvailableTickets]) VALUES (@Name, @Price, @AvailableTickets)";

                Database.ModifyData(sql,
                    Database.CreateParameter("@Name", ticketType.Name),
                    Database.CreateParameter("@Price", ticketType.Price),
                    Database.CreateParameter("@AvailableTickets", ticketType.AvailableTickets)
                );

                // Refresh data to retrieve ID of newly added item
                TicketTypes = GetTicketTypes();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EditTicketType(TicketType ticketType)
        {
            try
            {
                string sql = "UPDATE [TicketTypes] SET [Name] = @Name, [Price] = @Price, [AvailableTickets] = @AvailableTickets";
                sql += " WHERE [ID] = @ID";

                Database.ModifyData(sql,
                    Database.CreateParameter("@ID", ticketType.ID),
                    Database.CreateParameter("@Name", ticketType.Name),
                    Database.CreateParameter("@Price", ticketType.Price),
                    Database.CreateParameter("@AvailableTickets", ticketType.AvailableTickets)
                );

                // Refresh data to retrieve ID of newly added item
                TicketTypes = GetTicketTypes();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
