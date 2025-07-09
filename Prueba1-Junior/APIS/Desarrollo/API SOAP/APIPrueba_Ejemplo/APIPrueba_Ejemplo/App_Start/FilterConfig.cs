using System.Web;
using System.Web.Mvc;

namespace APIPrueba_Ejemplo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
