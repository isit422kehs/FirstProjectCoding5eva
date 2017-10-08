using ISIT422_MongodbNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ISIT422_MongodbNotes.Controllers
{
    public class NotesController : ApiController
    {
        Note[] notes = new Note[]
        {
            new Note { Id = 1, Priority = 3, Subject = "Wake up", Details = "Set alarm of 7:00 am and get out of bed." },
            new Note { Id = 2, Priority = 2, Subject = "Eat breakfast", Details = "Eat a healthy breakfast."},
            new Note { Id = 3, Priority = 5, Subject = "Go to work", Details = "Get to work before 9:00 am."}
        };

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
