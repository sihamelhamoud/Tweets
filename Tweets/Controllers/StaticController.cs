using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tweets.Models;

namespace Tweets.Controllers
{
    public class StaticController : ApiController
    {

        MongoDBEntities db = new MongoDBEntities();

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet]
        public IEnumerable<TweetsDB> Get(int id)
        {
            if (id == 1)
            {
                IEnumerable<TweetsDB> dep = (from f in db.GetAll()
                                orderby f.Tweet_Favorit descending
                                select  f).Take(5);
                dep = FillDGFive(dep);
                return dep;
            }
            else if (id == 2)
            {
                IEnumerable<TweetsDB> dep = (from f in db.GetAll()
                                orderby f.Tweet_Retweet descending

                                select f).Take(5);
                dep = FillDGFive(dep);
                return dep;
            }
            else if (id == 3)
            {
                IEnumerable<TweetsDB> dep = (from f in db.GetAll()
                                orderby f.Tweet_Mentiond descending
                                select f).Take(5);
                dep = FillDGFive(dep);
                return dep;
            }
            else if (id == 4)
            {
                IEnumerable<TweetsDB> dep = (from f in db.GetAll()
                                orderby f.Tweet_Followed descending
                                select f).Take(5);
                dep = FillDGFive(dep);
                return dep;
            }

            return db.GetAll();// "value";
        }

        private IEnumerable<TweetsDB> FillDGFive(IEnumerable<TweetsDB> dep)
        {
            IList<TweetsDB> result = new List<TweetsDB>();
            int count = 0;
            foreach (TweetsDB item in dep)
            {
                if (count < 5)
                {
                    result.Add(item);
                    count++;
                }
                else { break; }
            }
            return result;
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}