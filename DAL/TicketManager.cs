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
    public class TicketManager : ObservableObject
    {
        #region "Properties"
        private static readonly TicketManager _instance = new TicketManager();

        private TicketManager()
        {
        }

        public static TicketManager Instance
        {
            get
            {
                return _instance;
            }
        }

        private ObservableCollection<Ticket> _tickets;
        public ObservableCollection<Ticket> Tickets
        {
            get
            {
                if (_tickets == null)
                    _tickets = GetTickets();

                return _tickets;
            }
            set { _tickets = value; OnPropertyChanged("Tickets"); }
        }   
        #endregion

        // Do not use this method in WPF app
        // Only to be used when direct access to data from db is needed (website, API)
        public static ObservableCollection<Ticket> GetTickets()
        {
            try
            {
                string query = "SELECT [ID], [TicketHolder], [TicketHolderEmail], [Type], [Amount] FROM [Tickets]";
                DbDataReader reader = Database.GetData(query);

                return GetResults(reader);
            }
            catch (Exception)
            {
                return new ObservableCollection<Ticket>();
            }
        }

        private static ObservableCollection<Ticket> GetResults(DbDataReader reader)
        {
            ObservableCollection<Ticket> list = new ObservableCollection<Ticket>();
            while (reader.Read())
            {
                list.Add(CreateTicket(reader));
            }
            reader.Close();

            return list;
        }

        private static Ticket CreateTicket(IDataRecord row)
        {
            Ticket ticket = new Ticket();
            ticket.ID = !Convert.IsDBNull(row["ID"]) ? (int)row["ID"] : -1;
            ticket.TicketHolder = !Convert.IsDBNull(row["TicketHolder"]) ? row["TicketHolder"].ToString() : string.Empty;
            ticket.TicketHolderEmail = !Convert.IsDBNull(row["TicketHolderEmail"]) ? row["TicketHolderEmail"].ToString() : string.Empty;
            ticket.TicketType = !Convert.IsDBNull(row["Type"]) ? TicketTypeManager.GetTicketTypeByID((int)row["Type"]) : null;
            ticket.Amount = !Convert.IsDBNull(row["Amount"]) ? row["Amount"].ToString() : string.Empty;

            return ticket;
        }

        public static void InsertTicket(Ticket ticket)
        {
            try
            {
                string sql = "INSERT INTO [Tickets] ([TicketHolder], [TicketHolderEmail], [Type], [Amount]) VALUES (@TicketHolder, @TicketHolderEmail, @Type, @Amount)";

                Database.ModifyData(sql,
                    Database.CreateParameter("@TicketHolder", ticket.TicketHolder),
                    Database.CreateParameter("@TicketHolderEmail", ticket.TicketHolderEmail),
                    Database.CreateParameter("@Type", ticket.TicketType.ID),
                    Database.CreateParameter("@Amount", ticket.Amount)
                );
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddTicket(Ticket ticket)
        {
            try
            {
                InsertTicket(ticket);

                // Refresh data to retrieve ID of newly added item
                Tickets = GetTickets();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EditTicket(Ticket ticket)
        {
            try
            {
                string sql = "UPDATE [Tickets] SET [TicketHolder] = @TicketHolder, [TicketHolderEmail] = @TicketHolderEmail, [Type] = @Type, [Amount] = @Amount";
                sql += " WHERE [ID] = @ID";

                Database.ModifyData(sql,
                    Database.CreateParameter("@ID", ticket.ID),
                    Database.CreateParameter("@TicketHolder", ticket.TicketHolder),
                    Database.CreateParameter("@TicketHolderEmail", ticket.TicketHolderEmail),
                    Database.CreateParameter("@Type", ticket.TicketType.ID),
                    Database.CreateParameter("@Amount", ticket.Amount)
                );

                // Refresh data to retrieve ID of newly added item
                Tickets = GetTickets();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RefreshData()
        {
            Tickets = GetTickets();
        }
    }
}
