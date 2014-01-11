using DAL;
using FestivalWebsite.Models;
using FestivalWebsite.ViewModels;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FestivalWebsite.Controllers
{
    public class TicketController : Controller
    {
        //
        // GET: /Ticket/

        [Authorize]
        public ActionResult Index()
        {
            List<TicketTypeVM> ticketTypes = new List<TicketTypeVM>();
            foreach (TicketType ticketType in TicketTypeManager.GetTicketTypes())
            {
                ticketTypes.Add(new TicketTypeVM()
                {
                    TicketType = ticketType,
                    RemainingTickets = TicketTypeManager.CountTicketsRemainingForTicketType(ticketType.ID)
                });
            }

            return View(ticketTypes);
        }

        //
        // POST: /Ticket/

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string[] selectedTickets)
        {
            if (selectedTickets != null)
            {
                List<int> selectedTicketIDs = new List<int>();

                foreach (string selection in selectedTickets)
                {
                    if (!selection.Equals("false", StringComparison.CurrentCultureIgnoreCase))
                    {
                        int id = 0;
                        if (int.TryParse(selection, out id))
                        {
                            selectedTicketIDs.Add(id);
                        }
                    }
                }

                // Create viewmodel for selected ticket types
                List<TicketTypeAmountVM> ticketTypes = new List<TicketTypeAmountVM>();
                foreach (TicketType ticketType in TicketTypeManager.GetTicketTypes().Where(x => selectedTicketIDs.Contains(x.ID)))
                {
                    ticketTypes.Add(new TicketTypeAmountVM()
                    {
                        TicketType = ticketType,
                        RemainingTickets = TicketTypeManager.CountTicketsRemainingForTicketType(ticketType.ID),
                        Amount = 1
                    });
                }

                ViewBag.Customer = new Customer();

                return View("Amount", ticketTypes);
            }

            // Show form again with error message
            ModelState.AddModelError("NoTicketsSelected", "Please select at least one ticket type.");

            return Index();
        }

        //
        // POST: /Ticket/Order

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Order(ICollection<TicketTypeAmountVM> collection, Customer customer)
        {
            if (collection == null)
            {
                return RedirectToAction("Index");
            }

            // Check if the amounts are not exceeding the available tickets
            List<TicketTypeAmountVM> ticketTypes = new List<TicketTypeAmountVM>();

            foreach (TicketTypeAmountVM t in collection)
            {
                TicketType type = TicketTypeManager.GetTicketTypeByID(t.TicketType.ID);
                if (type != null)
                {
                    TicketTypeAmountVM item = new TicketTypeAmountVM()
                    {
                        TicketType = type,
                        RemainingTickets = TicketTypeManager.CountTicketsRemainingForTicketType(type.ID),
                        Amount = t.Amount
                    };

                    ticketTypes.Add(item);

                    // Show error if user tries to order more tickets than there are available
                    if (t.Amount > item.RemainingTickets && !ModelState.ContainsKey("TicketError"))
                    {
                        ModelState.AddModelError("TicketError", "You can't order more tickets than there are available.");
                    }
                }
            }

            if (!(ModelState.IsValidField("FirstName") && ModelState.IsValidField("LastName") && ModelState.IsValidField("Email")))
            {
                ModelState.AddModelError("TicketError", "Please fill in all fields.");
            }

            ViewBag.Customer = customer;

            // Only order tickets with an amount greater than 0
            List<TicketTypeAmountVM> validTicketTypes = ticketTypes.Where(x => x.Amount > 0).ToList();

            if (validTicketTypes.Count == 0)
            {
                ModelState.AddModelError("TicketError", "You must order at least one ticket.");
                return View("Amount", ticketTypes);
            }

            if (ModelState.ContainsKey("TicketError"))
            {
                return View("Amount", ticketTypes);
            }

            // Temporarily store chosen tickets and customer
            TempData["Tickets"] = validTicketTypes;
            TempData["Customer"] = customer;

            return View("Confirm", validTicketTypes);
        }

        //
        // POST: /Ticket/Confirm

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm()
        {
            // Get tickets the user ordered
            List<TicketTypeAmountVM> ticketTypes = TempData["Tickets"] as List<TicketTypeAmountVM>;
            Customer customer = TempData["Customer"] as Customer;

            // Generate and store tickets
            foreach (TicketTypeAmountVM t in ticketTypes)
            {
                Ticket ticket = new Ticket()
                {
                    TicketHolder = customer.FirstName + " " + customer.LastName,
                    TicketHolderEmail = customer.Email,
                    TicketType = t.TicketType,
                    Amount = t.Amount.ToString()
                };

                try
                {
                    TicketManager.InsertTicket(ticket);
                }
                catch (Exception)
                {
                    TempData["Error"] = "Something went wrong while processing your order, please try again later.";
                }
            }

            // Redirect to thank you page
            // Prevents overposting
            return RedirectToAction("Thanks");
        }

        //
        // GET: /Ticket/Thanks

        [Authorize]
        public ActionResult Thanks()
        {
            return View();
        }

        //
        // GET: /Ticket/Reservations

        [Authorize(Roles = "Admin")]
        public ActionResult Reservations()
        {
            List<TicketTypeVM> ticketTypes = new List<TicketTypeVM>();
            foreach (TicketType ticketType in TicketTypeManager.GetTicketTypes())
            {
                ticketTypes.Add(new TicketTypeVM()
                {
                    TicketType = ticketType,
                    RemainingTickets = TicketTypeManager.CountTicketsRemainingForTicketType(ticketType.ID)
                });
            }

            var tickets = TicketManager.GetTickets();

            double totalRevenue = 0;

            foreach (Ticket ticket in tickets)
            {
                totalRevenue += ticket.TicketType.Price * double.Parse(ticket.Amount);
            }

            ReservationsOverviewVM model = new ReservationsOverviewVM()
            {
                Tickets = tickets,
                TicketTypes = ticketTypes,
                TotalRevenue = totalRevenue
            };

            return View(model);
        }
    }
}
