using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ISIT422_Product_Notes.Controllers
{
    public class APIDeleteController : ApiController
    {
        public HttpResponseMessage DELETE(string id)
        {
            bool found = true;
            string subject = id;
            try
            {
              mongoDatabase = RetreiveMongohqDb();
              var mongoCollection = mongoDatabase.GetCollection("Notes");
              var query = Query.EQ("Subject", subject);
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
                HttpResponseMessage goodResponse = newHttpResponseMessage();
                goodResponse.StatusCode = HttpStatusCode.OK;
                return goodResponse;
            }
        }
    }
}
