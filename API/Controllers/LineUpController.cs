using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class LineUpController : ApiController
    {
        // GET api/lineup
        public IEnumerable<LineUpItem> Get()
        {
            IEnumerable<LineUpItem> items = LineUpManager.GetLineUpItems();
            foreach (LineUpItem item in items)
            {
                // Do not add pictures to the feed, client will lazily load them
                item.Band.Picture = null;
            }

            return items;
        }

        // GET api/lineup/5
        public LineUpItem Get(int id)
        {
            LineUpItem item = LineUpManager.Instance.LineUpItems.ToList().Find(x => x.ID == id);
            if (item != null)
                item.Band.Picture = null;

            return item;
        }

        // POST api/lineup
        public void Post([FromBody]string value)
        {
        }

        // PUT api/lineup/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/lineup/5
        public void Delete(int id)
        {
        }
    }
}
