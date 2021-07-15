using System.Web.Mvc;

namespace PHam_Le_Gia_Dai___1811064708___lab5
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}