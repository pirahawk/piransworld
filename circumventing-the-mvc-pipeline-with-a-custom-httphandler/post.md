If you are working with an ASP.Net MVC web application, you may run into a situation where you need to process and respond to certain incoming web requests outside of the normal MVC execution pipeline. If I am honest, I do not have a concrete use-case for why anyone would need to do this, as in my opinion the framework is be extensible enough to meet most of your needs. Hence only proceed if you have a very good reason (and please leave a comment below to share what it is).

For the sake of simplicity, I am going to illustrate this with a very simple example. Lets assume we have an existing MVC application where we want to identify a particular request that contains an `id` parameter, eg `/show-id?id=123`. We wish to respond without having to invoke the MVC execution pipeline, i.e. without the use of controllors, actions, views etc. We simply wish to extract the `id` and print it out as part of the response.

## Identify and intercept incoming requests to process
We begin with a way to identify the incoming web requests that we will respond to. In an MVC application this is done by registering a route (i.e. an instance of [RouteBase](https://msdn.microsoft.com/en-us/library/system.web.routing.routebase%28v=vs.110%29.aspx)), which outlines the pattern of the url to match against, from which the framework can establish which controller/action to invoke. All your routes should be registered with the `RouteTable` on application startup.

We create an extension of the `RouteBase` class to allow us to identify the route. On every incoming web request, the `GetRouteData` method is called on the RouteBase class to evaluate if the route should provide the necessary artifacts to respond to the request. Here we are simply checking to ensure that the request path matches our expected route `/show-id`. Returning a non-null instance of the `RouteData` class indicates a match.

[code language="csharp" title="PrintIdRoute.cs"]
public class PrintIdRoute : RouteBase
    {
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            if (httpContext.Request.Path.ToLower() != "/show-id")
                return null;
            return new RouteData();
        }
    }
[/code]

Next we need to register our custom route. Remember that the routing framework evaluates routes sequentially in the order in which they were registered. The first route to return a match is then tasked with responding to the request. To ensure our route is registered ahead of all the others, I edit the `Application_Start` method in my application's `Global.asax` file like so:
[code language="csharp" title="Global.asax"]
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
[/code]

**Note:** In an effort to maintain proper seperation of concern, you should not deal with route registration directly within the `Global.asax`. Ensure you apply proper software practises when actually implementing your solution.

## Create a Route Handler to respond to the pattern match
The `PrintIdRoute` class we have created should only be responsible for matching an incoming URL and return a non-null `RouteData` instance to signify a match. The responsibility of identifying who is responsible for processing the request lies with the [IRouteHandler](https://msdn.microsoft.com/en-us/library/system.web.routing.iroutehandler(v=vs.110).aspx) interface. It contains a sole method `GetHttpHandler` which is used to construct an instance of an [IHttpHandler](https://msdn.microsoft.com/en-us/library/system.web.ihttphandler%28v=vs.110%29.aspx) which in turn is responsible for processing the request. 

To illustrate the whole process, here is a *summarized excerpt* from the `ProcessRequest` method of the [UrlRoutingHandler](https://msdn.microsoft.com/en-us/library/system.web.routing.urlroutinghandler%28v=vs.110%29.aspx), which is called in response to an incoming web request:
[code language="csharp" title="System.Web.Routing.UrlRoutingHandler"]
protected virtual void ProcessRequest(HttpContextBase httpContext)
    {
      //Iterate through all the routes and see if any routes return a match
      RouteData routeData = this.RouteCollection.GetRouteData(httpContext);

      //Get the IRouteHandler from the returned routeData class
      IRouteHandler routeHandler = routeData.RouteHandler;

      //Capture the state of the current web request using a RequestContext
      RequestContext requestContext = new RequestContext(httpContext, routeData);

      //Get the IHttpHandler and allow it to process the request
      IHttpHandler httpHandler = routeHandler.GetHttpHandler(requestContext);
      VerifyAndProcessRequest(httpHandler, httpContext);
    }
[/code]

When mapping routes within the MVC framework, the values of `IRouteHandler` and `IHttpHandler` default to an instance of [System.Web.Mvc.MvcRouteHandler](https://msdn.microsoft.com/en-us/library/system.web.mvc.mvcroutehandler%28v=vs.118%29.aspx) and the [System.Web.Mvc.MvcHandler](https://msdn.microsoft.com/en-us/library/system.web.mvc.mvchandler%28v=vs.118%29.aspx) respectively. The `MvcRouteHandler`, prior to creating the `MvcHandler`, deals with other issues like configuring the session state behaviour. The `MvcHandler` on the other hand is what initializes the entire [MVC framework's processing pipeline](http://www.asp.net/mvc/overview/getting-started/lifecycle-of-an-aspnet-mvc-5-application), i.e. the process of invoking the controller & action to respond to the request etc.

We create our own 'IRouteHandler' implementation `PrintIdRouteHandler` which will serve up an instance of our `IHttpHandler` implementation `PrintIdHttpHandler`:

[code language="csharp" title="PrintIdRouteHandler.cs"]
public class PrintIdRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new PrintIdHttpHandler();
        }
    }
[/code]

[code language="csharp" title="PrintIdHttpHandler.cs"]
public class PrintIdHttpHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }

        public bool IsReusable { get; private set; }
    }
[/code]

We then ensure our custom routehandler is returned by our custom route upon a successful match. We do this by revisiting the `PrintIdRoute` class implementation:

[code language="csharp" title="PrintIdRoute.cs"]
public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            if (httpContext.Request.Path.ToLower() != "/show-id")
                return null;
            var routeData = new RouteData
            {
                RouteHandler = new PrintIdRouteHandler()
            };
            return routeData;
        }
[/code]

## Processing the request
The last step in the process involves implementing the `ProcessRequest` method within the `PrintIdHttpHandler` to respond to the incoming request.Before we can do this, remember our aim is to read and print out the value of the `id` url parameter within the result. We go back to our `PrintIdRoute` implementation:

[code language="csharp" title="PrintIdRoute.cs"]
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
    }
[/code]

We simply extract the value from the url parameter from the incoming request. The `RouteData` class offers a `Values` dictionary which allows a route to extract values from the incoming request and pass them further down the processing pipeline. Once this is done, we can instruct the `PrintIdRouteHandler` to pass this value to the constructor of the  `PrintIdHttpHandler` class during creation:

[code language="csharp"]
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

     public class PrintIdHttpHandler : IHttpHandler
    {
        private readonly int _id;

        public PrintIdHttpHandler(int id)
        {
            _id = id;
        }
        //.....
    }
[/code]

Finally we implement the `ProcessRequest` method to write the result to the response

[code language="csharp" title="PrintIdHttpHandler.cs"]
public void ProcessRequest(HttpContext context)
        {
            var message = string.Format("Recieved Id:{0}", _id);
            context.Response.Write(message);
        }
[/code]

Hence if everything is wired up correctly, we can now run the solution and navigate to the expected route we should see something similar to:
![screen1.PNG](http://piransworld.blob.core.windows.net/blog-images/circumventing-the-mvc-pipeline-with-a-custom-httphandler/screen1.PNG)

Although our example is extremely contrived for the sake of simplicity, you should note that by electing to bypass the normal MVC execution pipeline, we have elected to forgo all the convenience and security features offered by the framework. Hence I do not recommend using this approach unless you have a pressing need/concern that the framework cannot cater for. If anything I hope it has helped shed more light on how the MVC processing pipeline is invoked.

Feel free to download the source code for this example from [Github](https://github.com/pirahawk/piransworld/tree/master/circumventing-the-mvc-pipeline-with-a-custom-httphandler)