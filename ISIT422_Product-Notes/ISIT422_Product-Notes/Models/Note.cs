using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISIT422_Product_Notes.Models
{
    public class Note
    {
        int Id { get; set; }
        string Subject { get; set; }
        string Details { get; set; }
        int Priority { get; set; }
    }
}