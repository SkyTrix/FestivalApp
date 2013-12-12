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
            IEnumerable<Band> bands = BandManager.Instance.Bands;
            return View(bands);
        }

        public ActionResult Details(int id)
        {
            Band band = BandManager.Instance.Bands.Where(x => x.ID == id.ToString()).FirstOrDefault();
            if (band == null)
                return HttpNotFound();

            return View(band);
        }
    }
}
