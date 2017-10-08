using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISIT422_MongodbNotes.Models
{
    public class Note
    {
        [BsonId]
        public string Id { get; set; }//string ObjectId
        public string Subject { get; set; }
        public string Details { get; set; }
        public int Priority { get; set; }

    }
}