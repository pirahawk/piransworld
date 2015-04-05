using System.Web;
using System.Web.Routing;

namespace MyCustomHandler.Infrastructure
{
    public class PrintIdRoute : RouteBase
    {
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            if (httpContext.Request.Path.ToLower() != "/show-id")
                return null;

            var routeData = new RouteData
            {
                RouteHandler = new PrintIdRouteHandler()
            };
            var id = httpContext.Request.Params["id"];
            routeData.Values["id"] = id;
            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return null;
        }
    }
}