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
    public class GenreController : ApiController
    {
        // GET api/genre
        public IEnumerable<Genre> Get()
        {
            return GenreManager.Instance.Genres;
        }

        // GET api/genre/5
        public Genre Get(int id)
        {
            return GenreManager.Instance.Genres.ToList().Find(x => x.ID == id);
        }

        // POST api/genre
        public void Post([FromBody]string value)
        {
        }

        // PUT api/genre/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/genre/5
        public void Delete(int id)
        {
        }
    }
}
