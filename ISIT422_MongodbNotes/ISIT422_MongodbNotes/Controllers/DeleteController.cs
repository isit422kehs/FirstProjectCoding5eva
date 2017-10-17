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
    public class DeleteController : ApiController
    {
        MongoDatabase mongoDatabase;
        private MongoDatabase RetreiveMongohqDb()
        {
            MongoUrl myMongoURL = new MongoUrl(ConfigurationManager.ConnectionStrings["MongoHQ"].ConnectionString);
            MongoClient mongoClient = new MongoClient(myMongoURL);
            MongoServer server = mongoClient.GetServer();
            return server.GetDatabase("isit422_coding5eva");
        }

        [HttpDelete]
        public HttpResponseMessage Delete(string id)
        {
            bool found = true;
            string noteId = id;
            try
            {
                mongoDatabase = RetreiveMongohqDb();
                var mongoCollection = mongoDatabase.GetCollection("Notes");
                var query = Query.EQ("_id", noteId);
                WriteConcernResult results = mongoCollection.Remove(query);

                if (results.DocumentsAffected < 1)
                {
                    found = false;
                }
            }
            catch (Exception ex)
            {
                found = false;
            }
            if (!found)
            {
                HttpResponseMessage badResponse = new HttpResponseMessage();
                badResponse.StatusCode = HttpStatusCode.BadRequest;
                return badResponse;
            }
            else
            {
                HttpResponseMessage goodResponse = new HttpResponseMessage();
                goodResponse.StatusCode = HttpStatusCode.OK;
                return goodResponse;
            }
        }
    }
}


