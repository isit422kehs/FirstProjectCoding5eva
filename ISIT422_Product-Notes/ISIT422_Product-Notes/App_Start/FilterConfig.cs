using System.Web;
using System.Web.Mvc;

namespace ISIT422_Product_Notes
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
