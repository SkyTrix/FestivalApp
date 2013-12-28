using DAL;
using FestivalWebsite.Comparers;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FestivalWebsite.Controllers
{
    public class LineUpController : Controller
    {
        //
        // GET: /LineUp/

        public ActionResult Index(string sortOrder)
        {
            ViewBag.BandSort = sortOrder == "Band" ? "Band_desc" : "Band";
            ViewBag.DateSort = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewBag.StageSort = sortOrder == "Stage" ? "Stage_desc" : "Stage";

            IEnumerable<LineUpItem> lineUpItems = LineUpManager.GetLineUpItems();

            switch (sortOrder)
            {
                case "Band":
                    lineUpItems = lineUpItems.OrderBy(x => x.Band.Name);
                    break;
                case "Band_desc":
                    lineUpItems = lineUpItems.OrderByDescending(x => x.Band.Name);
                    break;
                case "Date_desc":
                    lineUpItems = lineUpItems.OrderByDescending(x => x.Date).ThenBy(x => x, new LineUpStartTimeComparer());
                    break;
                case "Stage":
                    lineUpItems = lineUpItems.OrderBy(x => x.Stage.Name).ThenBy(x => x, new LineUpStartTimeComparer());
                    break;
                case "Stage_desc":
                    lineUpItems = lineUpItems.OrderByDescending(x => x.Stage.Name).ThenBy(x => x, new LineUpStartTimeComparer());
                    break;
                default:
                    lineUpItems = lineUpItems.OrderBy(x => x.Date).ThenBy(x => x, new LineUpStartTimeComparer());
                    break;
            }

            return View(lineUpItems);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return HttpNotFound();

            LineUpItem lineUpItem = LineUpManager.GetLineUpItemByID(id.ToString());
            if (lineUpItem == null)
            {
                return HttpNotFound();
            }

            return View(lineUpItem);
        }
    }
}
