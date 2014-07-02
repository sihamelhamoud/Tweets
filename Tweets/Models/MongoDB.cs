using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Tweets.Models
{
    public class TweetsDB
    {
        //public int _id { get; set; }
        [Display(Name="ID")]
        public int TweetsId { get; set; }
        [Display(Name = "Name")]
        public string TweetsName { get; set; }
        [Display(Name = "Titel")]
        public string tweet_Title  {get ; set;}
        //public int Tweet_Id { get; set; }
        [Display(Name = "Description")]
        public string Tweet_Desc { get; set; }
        [Display(Name = "Waner")]
        public string Tweet_waner { get; set; }
        [Display(Name = "Retweet")]
        public int Tweet_Retweet { get; set; }
        [Display(Name = "Mentiond")]
        public int Tweet_Mentiond{ get; set; }
        [Display(Name = "Followed")]
        public int Tweet_Followed { get; set; }
        [Display(Name="Date")]
        public DateTime Tweet_DateTime { get; set; }
         [Display(Name = "Favorit")]
        public int Tweet_Favorit { get; set; }
    }
 

    public class MongoDBEntities : DbContext
    {
        public MongoDBEntities() : base("name=MongoConnection") { }

        static MongoServer server = MongoServer.Create(ConfigurationManager.ConnectionStrings["MongoConnection"].ConnectionString.ToString());
       
        MongoDatabase database = server.GetDatabase("MVCMongo");

        public List<TweetsDB> Tweet = new List<TweetsDB>();
        private WriteConcernResult result { get; set; }
        private bool hasError;


        public List<TweetsDB> GetAll()
        {
            List<TweetsDB> model = new List<TweetsDB>();
            var TweetsDBsList = database.GetCollection("TweetsDB").FindAll().AsEnumerable();
            model = (from TweetsDB in TweetsDBsList
                     select new TweetsDB
                     {
                         //_id = TweetsDB["_id"].AsObjectId,
                         TweetsId = TweetsDB["TweetsId"].AsInt32,
                         TweetsName = TweetsDB["TweetsName"].AsString,
                         Tweet_Desc = TweetsDB["Tweet_Desc"].AsString,
                         Tweet_Favorit = TweetsDB["Tweet_Favorit"].AsInt32,
                         Tweet_waner = TweetsDB["Tweet_waner"].AsString,
                         tweet_Title = TweetsDB["tweet_Title"].AsString,
                         Tweet_Followed = TweetsDB["Tweet_Followed"].AsInt32,
                         Tweet_DateTime = TweetsDB["Tweet_DateTime"].AsDateTime,
                         Tweet_Retweet = TweetsDB["Tweet_Retweet"].AsInt32,
                         Tweet_Mentiond = TweetsDB["Tweet_Mentiond"].AsInt32

                     }).ToList();
            return model;
        }

        public List<TweetsDB> Tweets
        {
            get
            {
                server.Connect();
                var collection = database.GetCollection<TweetsDB>("TweetsDB");
                return collection.FindAllAs<TweetsDB>().ToList();
            }
            set { Tweet = value; }
        }
        public TweetsDB CreateTweets(TweetsDB colleciton)
        {
           
            try
            {
                Tweet = GetAll();
                int Id = 0;
                if (Tweet.Count() > 0)
                    Id = Tweet.Max(x => x.TweetsId);
                Id += 1;
                MongoCollection<TweetsDB> MCollection = database.GetCollection<TweetsDB>("TweetsDB");
                BsonDocument doc = new BsonDocument { 
                    {"TweetsId",Id},
                    {"TweetsName", field2Str( colleciton.TweetsName)},
                    {"Tweet_DateTime", colleciton.Tweet_DateTime},
                    {"Tweet_Desc",field2Str( colleciton.Tweet_Desc)},
                    {"Tweet_Favorit", field2int(colleciton.Tweet_Favorit.ToString())},
                    {"Tweet_Followed",field2int(colleciton.Tweet_Followed.ToString())},
                    {"Tweet_Mentiond",field2int(colleciton.Tweet_Mentiond.ToString())},
                    {"Tweet_Retweet",field2int(colleciton.Tweet_Retweet.ToString())},
                    {"tweet_Title", field2Str(colleciton.tweet_Title)},
                    {"Tweet_waner", field2Str(colleciton.Tweet_waner)}
                    
                };

                IMongoQuery query = Query.EQ("TweetsName", field2Str(colleciton.TweetsName));
                var exists = MCollection.Find(query);
                if (exists.ToList().Count == 0)
                    result= MCollection.Insert(doc);
                
                if (result != null)
                {
                    hasError =(bool) result.HasLastErrorMessage;

                    if (!hasError)
                    {
                        return colleciton;
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.InternalServerError);
                    }
                }

            }
            catch { }
            return colleciton;
        }
        public bool EditTweets(TweetsDB collection)
        {
            try
            {
                MongoCollection<TweetsDB> MCollection = database.GetCollection<TweetsDB>("TweetsDB");
                IMongoQuery query = Query.EQ("TweetsId", collection.TweetsId);
                IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("TweetsName", collection.TweetsName)
                    .Set("Tweet_DateTime", collection.Tweet_DateTime)
                    .Set("Tweet_Desc", field2Str( collection.Tweet_Desc))
                    .Set("Tweet_Favorit", field2int(collection.Tweet_Favorit.ToString()))
                    .Set("Tweet_Followed", field2int(collection.Tweet_Followed.ToString()))
                    .Set("Tweet_Mentiond", field2int(collection.Tweet_Mentiond.ToString()))
                    .Set("Tweet_Retweet", field2int(collection.Tweet_Retweet.ToString()))
                    .Set("tweet_Title",field2Str( collection.tweet_Title))
                    .Set("Tweet_waner", field2Str( collection.Tweet_waner));
                 
               result =  MCollection.Update(query, update);
               if (result != null)
               {
                   hasError = result.HasLastErrorMessage;

                   if (!hasError)
                   {
                       return false;
                   }
                   else
                   {
                       throw new HttpResponseException(HttpStatusCode.InternalServerError);
                   }
               }
            }
            catch { }
            return true;
        }
        public void DeleteTweets( int ID)
        {
            try
            {
                MongoCollection<TweetsDB> MCollection = database.GetCollection<TweetsDB>("TweetsDB");
                IMongoQuery query = Query.EQ("TweetsId", ID);
                MCollection.Remove(query).ToString();
            }
            catch { }
        }

        public int field2int(string field)
        {
            int varint = 0;
            try
            {
                varint = Convert.ToInt32(field);
            }
            catch { varint = 0; }
            return varint;
        }

        public string field2Str(string field)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(field) || field == "" || field == null)
                    return " ";
            }
            catch { }
            return field;
        }

    }
}