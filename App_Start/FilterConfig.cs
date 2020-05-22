using System.Web;
using System.Web.Mvc;

namespace COLONY_THE_BUGTRACKER
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
