using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
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
                if (band != null)
                {
                    if (band.Picture != null)
                    {
                        return new FileContentResult(band.Picture, "image/jpeg");
                    }
                    else
                    {
                        var dir = Server.MapPath("~/Content/Images");
                        var path = Path.Combine(dir, "noimage.png");
                        return new FileStreamResult(new FileStream(path, FileMode.Open), "image/jpeg");
                    }
                }
            }

            return null;
        }
    }
}
