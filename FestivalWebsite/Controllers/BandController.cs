using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FestivalWebsite.Controllers
{
    public class BandController : Controller
    {
        //
        // GET: /Band/

        public ActionResult Index()
        {
            IEnumerable<Band> bands = BandManager.GetBands();
            return View(bands);
        }

        //
        // GET: /Band/Details

        public ActionResult Details(int? id)
        {
            if (id == null)
                return HttpNotFound();

            Band band = BandManager.GetBandByID(id.ToString());
            if (band == null)
            {
                return HttpNotFound();
            }

            return View(band);
        }
    }
}
