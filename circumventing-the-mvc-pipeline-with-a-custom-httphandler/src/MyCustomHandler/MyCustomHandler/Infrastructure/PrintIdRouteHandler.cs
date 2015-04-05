using System.Web;
using System.Web.Routing;

namespace MyCustomHandler.Infrastructure
{
    public class PrintIdRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            int id = 0;
            var o = requestContext.RouteData.Values["id"] as string;
            int.TryParse(o, out id);
            return new PrintIdHttpHandler(id);
        }
    }
}