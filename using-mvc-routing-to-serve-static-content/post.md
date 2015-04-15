The ASP.NET MVC routing system allows for the possibility of serving content like static *html* pages, ASP Webforms etc. There are a couple of ways you can wire this up within your MVC application.

## Configure your routes to detect and serve existing files
Every MVC application requires having to register a series of routes that dictate which controller/action combination is invoked in response to a web request. All routes need to be registered using the framework's `RouteCollection` object. Assume we have a static html page along the following path `~/Forms/Sample.html` within our MVC project:

[code language="html" title="~/Forms/Sample.html"]
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
</head>
<body>
    <h1>I am a static html page</h1>
</body>
</html>
[/code]

We register the routes in our MVC application using the `RouteCollection`. For simplicity, assume we register just a single route:

[code language="csharp"]
public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
[/code]

Here, you need to enable the `[RouteExistingFiles](https://msdn.microsoft.com/en-us/library/system.web.routing.routecollection.routeexistingfiles%28v=vs.110%29.aspx)` property on the `RouteCollection`. This simply instructs the routing system to match incoming request paths against local file paths and serve the respective file if it exists.  

[code language="csharp"]
public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = true;
            //..........
        }
    }
[/code]

You may now run the application and navigate to the static file via the browser. Depending on your project settings, the url for this should look something similar to `http://localhost:11794/Forms/Sample.html`

![screen1.PNG](http://az743082.vo.msecnd.net/blog-images/using-mvc-routing-to-serve-static-content/screen1.PNG)

## Map a Page Route
The above approach has some limitations to it: 
* It is potentially unsafe. You can unknowingly end up providing access to content that should not be directly accessible. 
* It will not work in the instance your solution has any ASP Webform pages that need to be accessed. This is because the ASP webforms are designed to follow a strict [life cycle during the course of their activation](https://msdn.microsoft.com/en-us/library/ms178472%28v=vs.85%29.aspx).

The `RouteCollection` class offers another alternative to route to existing files. The `MapPageRoute` method allows assigning an incoming route to a physical file.




