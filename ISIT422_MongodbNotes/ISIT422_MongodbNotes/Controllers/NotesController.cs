using ISIT422_MongodbNotes.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ISIT422_MongodbNotes.Controllers
{
    public class NotesController : ApiController
    {
        MongoDatabase mongoDatabase;

        private MongoDatabase RetreiveMongohqDb()
        {
            //MongoUrl myMongoURL = new MongoUrl(ConfigurationManager.ConnectionStrings["MongoHQ"].ConnectionString);
            MongoClient mongoClient = new MongoClient("mongodb://db_elizabeth:741123@ds044689.mlab.com:44689/isit422_coding5eva");
            MongoServer server = mongoClient.GetServer();
            return mongoClient.GetServer().GetDatabase("isit422_coding5eva");
        }
    

    public IEnumerable<Note> GetAllNotes()
        {
            mongoDatabase = RetreiveMongohqDb();

            List<Note> noteList = new List<Note>();
            try
            {
                var mongoList = mongoDatabase.GetCollection("Notes").FindAll().AsEnumerable();
                noteList = (from note in mongoList
                            select new Note
                            {
                                Id = note["_id"].AsString,
                                Subject = note["Subject"].AsString,
                                Details = note["Details"].AsString,
                                Priority = note["Priority"].AsInt32

                            }).ToList();
            }
            catch (Exception)
            {
                throw new ApplicationException("failed to get data from Mongo");
            }
            noteList.Sort(); // comment this out until you implement the IComparable<Note>
                             // interface definition to your Note class,
            return noteList;  // ASP API will convert a List of Note objects to json
        }

    

    public IHttpActionResult GetNote(string id)  // make sure its string
        {
            mongoDatabase = RetreiveMongohqDb();

            List<Note> noteList = new List<Note>();
            try
            {
                var mongoList = mongoDatabase.GetCollection("Notes").FindAll().AsEnumerable();
                noteList = (from nextNote in mongoList
                            select new Note
                            {
                                Id = nextNote["_id"].AsString,
                                Subject = nextNote["Subject"].AsString,
                                Details = nextNote["Details"].AsString,
                                Priority = nextNote["Priority"].AsInt32,
                            }).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }

            var note = noteList.FirstOrDefault((p) => p.Subject == id);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

    }

}
