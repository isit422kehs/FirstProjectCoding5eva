using ISIT422_MongodbNotes.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ISIT422_MongodbNotes.Controllers
{
    public class SaveController : ApiController
    {
        MongoDatabase mongoDatabase;
        private MongoDatabase RetreiveMongohqDb()
        {
            //MongoUrl myMongoURL = new MongoUrl(ConfigurationManager.ConnectionStrings["MongoHQ"].ConnectionString);
            MongoClient mongoClient = new MongoClient("mongodb://db_elizabeth:741123@ds044689.mlab.com:44689/isit422_coding5eva");
            MongoServer server = mongoClient.GetServer();
            return mongoClient.GetServer().GetDatabase("isit422_coding5eva");
        }
    
    [HttpPost]
        public Note Save(Note newNote)
        {
            mongoDatabase = RetreiveMongohqDb();
            var noteList = mongoDatabase.GetCollection("Notes");
            WriteConcernResult result;
            bool hasError = false;
            if (string.IsNullOrEmpty(newNote.Id))
            {
                newNote.Id = ObjectId.GenerateNewId().ToString();
                result = noteList.Insert<Note>(newNote);
                hasError = result.HasLastErrorMessage;
            }
            else
            {
                IMongoQuery query = Query.EQ("_id", newNote.Id);
                IMongoUpdate update = Update
                    .Set("Subject", newNote.Subject)
                    .Set("Details", newNote.Details)
                    .Set("Priority", newNote.Priority);
                result = noteList.Update(query, update);
                hasError = result.HasLastErrorMessage;
            }
            if (!hasError)
            {
                return newNote;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}
