using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace ISIT422_MongodbNotes.Models
{
    public class Note : IComparable
    {
        [BsonId]
        public string Id { get; set; }
        public string Subject { get; set; }
        public string Details { get; set; }
        public int Priority { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Note otherNote = obj as Note;
            if (otherNote != null)
            {
                return this.Priority.CompareTo(otherNote.Priority);
            }
            else {
                throw new ArgumentException();
            }
            
        }
    }
}