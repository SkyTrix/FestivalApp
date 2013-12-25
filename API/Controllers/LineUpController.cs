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
            return LineUpManager.Instance.LineUpItems;
        }

        // GET api/lineup/5
        public LineUpItem Get(int id)
        {
            return LineUpManager.Instance.LineUpItems.ToList().Find(x => x.ID == id);
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
