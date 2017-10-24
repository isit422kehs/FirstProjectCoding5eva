using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using ISIT422_MongodbNotes.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Web.Http;

namespace ISIT422_MongodbNotes.Controllers
{
    public class NotesController : ApiController
    {
        //string collectionName = "Notes";  // production
        string collectionName = "TestCollection";  // testing

        bool testing = false;
        List<Note> noteList = new List<Note>();
        // add default controller for normal opperation
        public NotesController()
        {
            testing = false;
        }

        // add controller that lets you pass in a fake db for testing
        public NotesController(List<Note> FakeDataList)
        {
            noteList = FakeDataList;
            testing = true;
        }


        MongoDatabase mongoDatabase;

        public IEnumerable<Note> GetAllNotes()

        {
            if (!testing)  // if not testing, read data from real db
            {
                mongoDatabase = RetreiveMongohqDb();

                try
                {
                    var mongoList = mongoDatabase.GetCollection(collectionName).FindAll().AsEnumerable();
                    noteList = (from note in mongoList
                                select new Note
                                {
                                    Id = note["_id"].AsString, //((ObjectId)note["_id"]).ToString(),
                                    Subject = note["Subject"].AsString,
                                    Details = note["Details"].AsString,
                                    Priority = note["Priority"].AsInt32

                                }).ToList();
                }
                catch (Exception)
                {
                    throw new ApplicationException("failed to get data from Mongo");
                }
            }

            noteList.Sort(); // comment this out until you implement the IComparable<Note>
            return noteList;  // ASP API will convert a List of Note objects to json
        }


        public IHttpActionResult GetNote(string id)  // make sure its string
        {
            if (!testing)
            {
                mongoDatabase = RetreiveMongohqDb();

                try
                {
                    var mongoList = mongoDatabase.GetCollection(collectionName).FindAll().AsEnumerable();
                    noteList = (from nextNote in mongoList
                                select new Note
                                {
                                    Id = nextNote["_id"].AsString, //((ObjectId)nextNote["_id"]).ToString(), 
                                    Subject = nextNote["Subject"].AsString,
                                    Details = nextNote["Details"].AsString,
                                    Priority = nextNote["Priority"].AsInt32,
                                }).ToList();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            var note = noteList.FirstOrDefault((p) => p.Subject == id);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }


        [HttpDelete]
        public HttpResponseMessage Delete(string id)
        {
            bool found = true;
            string noteId = id;

            if (!testing)
            {
                mongoDatabase = RetreiveMongohqDb();
                
                try
                {
                    mongoDatabase = RetreiveMongohqDb();
                    var mongoCollection = mongoDatabase.GetCollection(collectionName);
                    var query = Query.EQ("Subject", noteId);
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


        [HttpPost]
        public Note Save(Note newNote)
        {
            mongoDatabase = RetreiveMongohqDb();
            var noteList = mongoDatabase.GetCollection(collectionName);
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


        private MongoDatabase RetreiveMongohqDb()
        {
            string connString = "mongodb://db_elizabeth:741123@ds044689.mlab.com:44689/isit422_coding5eva";
            MongoUrl myMongoURL = new MongoUrl(connString);
            MongoClient mongoClient = new MongoClient(myMongoURL);
            MongoServer server = mongoClient.GetServer();
            return mongoClient.GetServer().GetDatabase("isit422_coding5eva");
        }
    }
}
