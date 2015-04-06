using System.Web.Mvc;
using System.Web.Routing;
using MyCustomHandler.Infrastructure;

namespace MyCustomHandler
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            var routeCollection = RouteTable.Routes;
            
            //add custom route before registering all other routes
            routeCollection.Add(new PrintIdRoute());
            
            //Register all other MVC routes
            RouteConfig.RegisterRoutes(routeCollection);
        }
    }
}
