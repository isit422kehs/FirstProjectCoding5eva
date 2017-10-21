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
        string collectionName = "NotesTest";  // testing

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
                    var mongoList = mongoDatabase.GetCollection("Notes").FindAll().AsEnumerable();
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
                             // interface definition to your Note class,
            return noteList;  // ASP API will convert a List of Note objects to json
        }

    

    public IHttpActionResult GetNote(string id)  // make sure its string
        {
            if (!testing)
            {
                mongoDatabase = RetreiveMongohqDb();


                try
                {
                    var mongoList = mongoDatabase.GetCollection("Notes").FindAll().AsEnumerable();
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
            var note = noteList.FirstOrDefault((p) => p.Id == id);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        private MongoDatabase RetreiveMongohqDb()
        {
            MongoUrl myMongoURL = new MongoUrl(ConfigurationManager.ConnectionStrings["MongoHQ"].ConnectionString);
            MongoClient mongoClient = new MongoClient(myMongoURL);
            MongoServer server = mongoClient.GetServer();
            return server.GetDatabase("isit422_coding5eva");
        }
    }
}
