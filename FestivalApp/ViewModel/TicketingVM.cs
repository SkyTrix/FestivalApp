using DAL;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using FestivalApp.View;
using GalaSoft.MvvmLight.Command;
using Models;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace FestivalApp.ViewModel
{
    class TicketingVM : ObservableObject, IPage
    {
        private TicketManager _ticketManager;
        public TicketManager TicketManager
        {
            get
            {
                if (_ticketManager == null)
                    _ticketManager = TicketManager.Instance;

                return _ticketManager;
            }
            set { _ticketManager = value; OnPropertyChanged("TicketManager"); }
        }

        private ObservableCollection<TicketTypeVM> _ticketTypes;
        public ObservableCollection<TicketTypeVM> TicketTypes
        {
            get
            {
                if (_ticketTypes == null)
                {
                    // Use a presentation model so we can display both the ticket types and the tickets remaining for the type
                    ObservableCollection<TicketTypeVM> ticketTypes = new ObservableCollection<TicketTypeVM>();

                    foreach (TicketType ticketType in TicketTypeManager.Instance.TicketTypes)
                    {
                        ticketTypes.Add(new TicketTypeVM()
                        {
                            TicketType = ticketType,
                            RemainingTickets = TicketTypeManager.CountTicketsRemainingForTicketType(ticketType.ID)
                        });
                    }

                    _ticketTypes = ticketTypes;
                }

                return _ticketTypes;
            }
            set { _ticketTypes = value; OnPropertyChanged("TicketTypes"); }
        }

        private TicketTypeVM _selectedTicketTypeVM;
        public TicketTypeVM SelectedTicketTypeVM
        {
            get { return _selectedTicketTypeVM; }
            set { _selectedTicketTypeVM = value; OnPropertyChanged("SelectedTicketTypeVM"); }
        }

        private Ticket _selectedTicket;
        public Ticket SelectedTicket
        {
            get { return _selectedTicket; }
            set { _selectedTicket = value; OnPropertyChanged("SelectedTicket"); }
        }
        
        public string Name
        {
            get { return "Ticketing"; }
        }

        public ICommand AddTicketTypeCommand
        {
            get { return new RelayCommand(AddTicketType); }
        }

        private void AddTicketType()
        {
            TicketTypeWindow window = new TicketTypeWindow();
            window.DataContext = new AddTicketTypeVM();
            if (window.ShowDialog() == true)
            {
                // Force reload of ticket type UI
                TicketTypes = null;
            }
        }

        public ICommand EditTicketTypeCommand
        {
            get { return new RelayCommand(EditTicketType, CanEditTicketType); }
        }

        private bool CanEditTicketType()
        {
            return SelectedTicketTypeVM != null && SelectedTicketTypeVM.TicketType != null;
        }

        private void EditTicketType()
        {
            TicketTypeWindow window = new TicketTypeWindow();
            EditTicketTypeVM viewModel = new EditTicketTypeVM();
            viewModel.TicketType = SelectedTicketTypeVM.TicketType.Copy();
            window.DataContext = viewModel;
            window.Title = "Ticket type wijzigen";
            if (window.ShowDialog() == true)
            {
                // Force reload of ticket type UI
                TicketTypes = null;
            }
        }

        public ICommand AddReservationCommand
        {
            get { return new RelayCommand(AddReservation); }
        }

        private void AddReservation()
        {
            ReservationWindow window = new ReservationWindow();
            window.DataContext = new AddReservationVM();
            if (window.ShowDialog() == true)
            {
                // Force reload of ticket type UI
                TicketTypes = null;
            }
        }

        public ICommand EditReservationCommand
        {
            get { return new RelayCommand(EditReservation, CanEditReservation); }
        }

        private bool CanEditReservation()
        {
            return SelectedTicket != null;
        }

        private void EditReservation()
        {
            ReservationWindow window = new ReservationWindow();
            EditReservationVM viewModel = new EditReservationVM();
            viewModel.Ticket = SelectedTicket.Copy();
            window.DataContext = viewModel;
            window.Title = "Reservatie wijzigen";
            if (window.ShowDialog() == true)
            {
                // Force reload of ticket type UI
                TicketTypes = null;
            }
        }

        public ICommand GenerateTicketsCommand
        {
            get { return new RelayCommand(GenerateTickets, CanGenerateTickets); }
        }

        private bool CanGenerateTickets()
        {
            return TicketManager.Tickets != null && TicketManager.Tickets.Count > 0;
        }

        private void GenerateTickets()
        {
            try
            {
                VistaFolderBrowserDialog ofd = new VistaFolderBrowserDialog();
                ofd.Description = "Waar wenst u de tickets op te slaan?";
                ofd.UseDescriptionForTitle = true;
                if (ofd.ShowDialog() == true)
                {
                    foreach (Ticket ticket in TicketManager.Tickets)
                    {
                        string filename = ofd.SelectedPath + Path.DirectorySeparatorChar + ticket.ID + "_" + ticket.TicketHolder + ".docx";
                        File.Copy("ReservationTemplate.docx", filename, true);

                        WordprocessingDocument newdoc = WordprocessingDocument.Open(filename, true);
                        IDictionary<String, BookmarkStart> bookmarks = new Dictionary<String, BookmarkStart>();

                        foreach (BookmarkStart bms in newdoc.MainDocumentPart.RootElement.Descendants<BookmarkStart>())
                        {
                            bookmarks[bms.Name] = bms;
                        }

                        Run run = new Run(new Text("TK" + ticket.ID.ToString()));
                        RunProperties prop = new RunProperties();
                        RunFonts font = new RunFonts() { Ascii = "Free 3 of 9 Extended", HighAnsi = "Free 3 of 9 Extended" };
                        FontSize size = new FontSize() { Val = "192" };
                        prop.Append(font);
                        prop.Append(size);
                        run.PrependChild<RunProperties>(prop);
                        bookmarks["TicketNumber"].Parent.InsertAfter<Run>(run, bookmarks["TicketNumber"]);

                        Run run2 = new Run(new Text(ticket.TicketType.Name));
                        RunProperties prop2 = new RunProperties();
                        FontSize size2 = new FontSize() { Val = "72" };
                        prop2.Append(size2);
                        run2.PrependChild<RunProperties>(prop2);
                        bookmarks["TicketType"].Parent.InsertAfter<Run>(run2, bookmarks["TicketType"]);

                        Run run3 = new Run(new Text(ticket.TicketHolder));
                        RunProperties prop3 = new RunProperties();
                        FontSize size3 = new FontSize() { Val = "28" };
                        prop3.Append(size3);
                        run3.PrependChild<RunProperties>(prop3);
                        bookmarks["TicketHolder"].Parent.InsertAfter<Run>(run3, bookmarks["TicketHolder"]);

                        Run run4 = new Run(new Text(ticket.TicketHolderEmail));
                        RunProperties prop4 = new RunProperties();
                        FontSize size4 = new FontSize() { Val = "28" };
                        prop4.Append(size4);
                        run4.PrependChild<RunProperties>(prop4);
                        bookmarks["TicketHolderEmail"].Parent.InsertAfter<Run>(run4, bookmarks["TicketHolderEmail"]);

                        Run run5 = new Run(new Text("Aantal personen: " + ticket.Amount));
                        RunProperties prop5 = new RunProperties();
                        FontSize size5 = new FontSize() { Val = "28" };
                        prop5.Append(size5);
                        run5.PrependChild<RunProperties>(prop5);
                        bookmarks["Amount"].Parent.InsertAfter<Run>(run5, bookmarks["Amount"]);

                        Run run6 = new Run(new Text((ticket.TicketType.Price * Int32.Parse(ticket.Amount)).ToString("C2")));
                        RunProperties prop6 = new RunProperties();
                        FontSize size6 = new FontSize() { Val = "28" };
                        prop6.Append(size6);
                        run6.PrependChild<RunProperties>(prop6);
                        bookmarks["Price"].Parent.InsertAfter<Run>(run6, bookmarks["Price"]);

                        newdoc.Close();
                    }

                    // open the selected folder in explorer
                    Process process = new Process();
                    process.StartInfo.FileName = "explorer.exe";
                    process.StartInfo.Arguments = ofd.SelectedPath;
                    process.Start();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
