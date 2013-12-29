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

        public ActionResult Index(string sortOrder, string date, string stage)
        {
            IEnumerable<LineUpItem> lineUpItems = LineUpManager.GetLineUpItems();
            IEnumerable<DateTime> festivalDates = FestivalManager.GetFestival().FestivalDates;
            IEnumerable<Stage> stages = StageManager.GetStages();

            ViewBag.BandSort = sortOrder == "Band" ? "Band_desc" : "Band";
            ViewBag.DateSort = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewBag.StageSort = sortOrder == "Stage" ? "Stage_desc" : "Stage";

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

            // Make sure selected date exists, don't trust user
            if (!string.IsNullOrEmpty(date) && festivalDates.ToList().FindAll(x => x.ToShortDateString().Equals(date)).Count != 0)
            {
                lineUpItems = lineUpItems.ToList().FindAll(x => x.Date.ToShortDateString() == date);
                ViewBag.SelectedDate = date;
            }

            // Make sure selected date exists, don't trust user
            if (!string.IsNullOrEmpty(stage) && stages.ToList().Find(x => x.ID.ToString() == stage) != null)
            {
                lineUpItems = lineUpItems.ToList().FindAll(x => x.Stage.ID.ToString() == stage);
                ViewBag.SelectedStage = stage;
            }

            // Add error message if there are no results
            if (lineUpItems.ToList().Count == 0)
            {
                ViewBag.Error = "There are no results for your selection.";
            }

            List<SelectListItem> dates = new List<SelectListItem>();
            foreach (DateTime dt in festivalDates)
            {
                dates.Add(new SelectListItem() { Text = dt.ToShortDateString(), Value = dt.ToShortDateString() });
            }

            ViewBag.Date = dates;
            ViewBag.Stage = new SelectList(stages, "ID", "Name", stage);

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
