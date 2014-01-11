using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;

namespace FestivalWebsite.Controllers
{
    public class RssController : Controller
    {
        //
        // GET: /Rss/

        public ActionResult Index()
        {
            List<NewsItem> newsItems = NewsItemManager.GetNewsItems();
            List<SyndicationItem> items = new List<SyndicationItem>();

            foreach (NewsItem item in newsItems)
            {
                items.Add(new SyndicationItem(item.Title, item.Content, null));
            }

            Festival festival = FestivalManager.Instance.Festival;
            SyndicationFeed feed = new SyndicationFeed(festival.Name + " Nieuws", null, Request.Url, items);

            return new RssResult(feed);
        }
    }
}
