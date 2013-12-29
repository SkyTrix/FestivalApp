using DAL;
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

        public ActionResult Index()
        {
            return View();
        }

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

            foreach(Ticket ticket in tickets)
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
