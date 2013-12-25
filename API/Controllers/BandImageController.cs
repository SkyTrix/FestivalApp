using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace API.Controllers
{
    public class BandImageController : Controller
    {
        //
        // GET: /BandImage/

        public ActionResult Index()
        {
            return HttpNotFound();
        }

        public FileResult Get(int? id)
        {
            if (id != null)
            {
                Band band = BandManager.GetBandByID(id.ToString());
                if (band != null && band.Picture != null)
                {
                    return new FileContentResult(band.Picture, "image/jpeg");
                }
            }

            return null;
        }
    }
}
