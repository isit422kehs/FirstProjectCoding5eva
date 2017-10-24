using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Web.Http;


using ISIT422_MongodbNotes.Controllers;
using ISIT422_MongodbNotes.Models;
using System.Web.Http.Results;

namespace ISIT422_MongodbNotes.Tests
{
    [TestClass]
    public class TestNotesController
    {
        List<Note> noteList = new List<Note>();

        // method used to generate fake List of valid data
        private List<Note> GenerateFakeDataList()
        {
            List<Note> workingList = new List<Note>();
            for (int i = 0; i < 3; i++)
            {
                Note nextNote = new Note();

                nextNote.Id = i.ToString();
                nextNote.Subject = "Test" + i.ToString();
                nextNote.Details = "Test" + i.ToString() + " Details";
                nextNote.Priority = i;
                workingList.Add(nextNote);
            }
            return workingList;
        }

        //=======================================================================
        // test first API   GetAllNotes()
        [TestMethod]
        // first test local logic, using fake data
        public void GetAllFakeNotes_ShouldReturnAllNotes()
        {
            List<Note> testNotes = GenerateFakeDataList();
            var controller = new NotesController(testNotes); // use 1 of 2 constructors

            var result = controller.GetAllNotes() as List<Note>;
            Assert.AreEqual(testNotes.Count, result.Count);
        }

        [TestMethod]
        // now test against test data in mongo
        public void GetAllMongoNotes_ShouldReturnAllNotes()
        {
            // need to modify Controller to point to NotesTest
            List<Note> testNotes = GenerateFakeDataList();
            var controller = new NotesController(); // use the other constructor

            var result = controller.GetAllNotes() as List<Note>;
            Assert.AreEqual(testNotes.Count, result.Count);
        }

        //=======================================================================
        // test 2nd API   GetNote(string id)
        [TestMethod]
        // first test local logic, using fake data
        public void GetFakeNote_ShouldReturnParticularNote()
        {
            List<Note> testNotes = GenerateFakeDataList();
            var controller = new NotesController(testNotes); // use 1 of 2 constructors

            IHttpActionResult result = controller.GetNote("Test2");
            var contentResult = result as OkNegotiatedContentResult<Note>;

            Assert.AreEqual(testNotes[2].Subject, contentResult.Content.Subject);
        }

        [TestMethod]
        // now test against test data in mongo
        public void GetMongoNote_ShouldReturnParticularNote()
        {
            List<Note> testNotes = GenerateFakeDataList();
            var controller = new NotesController(); // use other constructors

            IHttpActionResult result = controller.GetNote("Test2");
            var contentResult = result as OkNegotiatedContentResult<Note>;

            Assert.AreEqual(testNotes[2].Subject, contentResult.Content.Subject);
        }

        //=======================================================================
        // test API Get one note that doesn't exist, assert that not found is true
        [TestMethod]
        // first test local logic, using fake data
        public void GetFakeNote_ShouldReturnNotFound()
        {
            List<Note> testNotes = GenerateFakeDataList();
            var controller = new NotesController(testNotes); // use 1 of 2 constructors

            IHttpActionResult result = controller.GetNote("Test5-Silvia");

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        // now test against test data in mongo
        public void GetMongoNote_ShouldReturnNotFound()
        {
            List<Note> testNotes = GenerateFakeDataList();
            var controller = new NotesController(); // use other constructors

            IHttpActionResult result = controller.GetNote("Test5-Silvia");

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

        }

        //=======================================================================
        // test 4th API   Delete(string id)
        [TestMethod]
        // first test local logic, using fake data
        public void GetFakeNote_ShouldFindNoteAndDelete()
        {
            var controller = new NotesController(); // use 1 of 2 constructors
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var fakeNoteSubj = "Test1";
            var response = controller.Delete(fakeNoteSubj);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        // now test against test data in mongo
        public void GetMongoNote_ShouldFindNoteAndDelete()
        {
            List<Note> testNotes = GenerateFakeDataList();
            var controller = new NotesController(testNotes); // use other constructor
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var noteSubj = "Test1";
            var response = controller.Delete(noteSubj);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public void AddFakeNote_ShouldAddNote()
        {

            var controller = new NotesController(GenerateFakeDataList());
            Note addNote = new Note();
            addNote.Id = "asdf";
            addNote.Subject = "Test1";
            addNote.Details = "Test1 details";
            addNote.Priority = 1;

            var response = controller.Save(addNote);

            IHttpActionResult result = controller.GetNote("Test1");
            //var delete = controller.Delete("Test1");
            var result2 = result as OkNegotiatedContentResult<Note>;
            Assert.AreEqual(response.Subject, result2.Content.Subject);
        }
        [TestMethod]
        public void AddMongoNote_ShouldAddRecord()
        {

            var controller = new NotesController();
            Note addNote = new Note();
            addNote.Id = "";
            addNote.Subject = "Test1";
            addNote.Details = "Test1 details";
            addNote.Priority = 1;

            var response = controller.Save(addNote);

            IHttpActionResult result = controller.GetNote("Test1");
            var delete = controller.Delete("Test1 ");
            var result2 = result as OkNegotiatedContentResult<Note>;

            Assert.AreEqual(response.Subject, result2.Content.Subject);
        }

    }
}
