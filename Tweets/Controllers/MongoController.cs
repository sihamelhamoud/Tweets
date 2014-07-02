using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

using System.Net;
using Tweets.Models;

namespace Tweets.Controllers
{
    public class MongoController : ApiController
    {
        MongoDBEntities db = new MongoDBEntities();


        //Get All Tweets
        [HttpGet]
        public IEnumerable<TweetsDB> GetAllTweets()
        {
            return db.GetAll();
        }

        //Get Tweets by id
        [HttpGet]
        public TweetsDB GetTweetsById(int id)
        {
            TweetsDB dep = (from f in db.GetAll()
                            where f.TweetsId == id
                            select f).First();            
            return dep;
        }

        //Add Tweets
        [HttpPost]
        public TweetsDB AddTweets(TweetsDB TW)
        {
            TW.Tweet_DateTime = DateTime.Now;
            TW = db.CreateTweets(TW); 

            return TW;
        }
        //Update Tweets
        [HttpPut]
        public void UpdateTweets(TweetsDB TW)
        {

            if (!db.EditTweets(TW))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        //Delete Tweets
        [HttpDelete]
        public void DeleteTweets(int id)
        {
            db.DeleteTweets(id);
        }
    }    
    
}
