#Using Ninject with ASP.NET Web API 2

I thought I would write a quick post on how to use [Ninject](http://www.ninject.org/) as an _IOC_ container when using [Web API 2](http://www.asp.net/web-api). I must admit, this was not a great experience when I tried it first, mainly as I was unaware of all the supplimentary _Ninject_ packages I needed to get this working. Hopefully this post saves you from having to go through the same pain. 

**Please note** that my approach only touches how to **use Ninject when using IIS/IIS-Express as a hosting layer**.

## Setting up the Web API solution
We start by creating a very simple Web API solution. Using the visual studio templates (note: I am using [VS2015 community edition](https://www.visualstudio.com/en-us/products/visual-studio-community-vs.aspx)) I create a new ASP.NET Web Application project, using the `Empty Project` template with references for all `Web API` assemblies. My project structure currently looks like this:

![screen1.PNG](http://az743082.vo.msecnd.net/blog-images/using-ninject-with-web-api-2/screen1.JPG)

If  you get a list of all packages installed at this point, you should see something along the lines of

```
PM> get-package

Id                                  Versions                                 ProjectName
--                                  --------                                 -----------                                                                         
Microsoft.AspNet.WebApi             {5.2.3}                                  ApiWithNinject                                            
Microsoft.AspNet.WebApi.Client      {5.2.3}                                  ApiWithNinject
Microsoft.AspNet.WebApi.Core        {5.2.3}                                  ApiWithNinject
Microsoft.AspNet.WebApi.WebHost     {5.2.3}                                  ApiWithNinject
Microsoft.CodeDom.Providers.DotN... {1.0.0}                                  ApiWithNinject
Microsoft.Net.Compilers             {1.0.0}                                  ApiWithNinject
Newtonsoft.Json                     {6.0.4}                                  ApiWithNinject     
```

## Setting up the Message Controller example
For the sake of brevity, I am going to use a simple controller callect `MessageController`. The intent here is for the api controller to retrieve and serve a message from an `IMessageService` instance, which will be injected via the controllers constructor (by Ninject). These are the main classes that make up my example:

[code language="csharp" title="IMessageService"]
public interface IMessageService
    {
        string Message();
    }
[/code]


[code language="csharp" title="MessageService"]
public class MessageService : IMessageService
    {
        public string Message()
        {
            return "Hello from the Message Service";
        }
    }
[/code]

[code language="csharp" title="MessageController"]
    public class MessageController : ApiController
    {
        private IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [Route("api/message")]
        public HttpResponseMessage GetMessage() {
            return Request.CreateResponse(HttpStatusCode.OK, _messageService.Message());
        }
    }
[/code]

**Note** that I am using `HttpAttributeRoutes` to specify the routes. Ensure it is enabled when configuring web api on startup (from the `Global.asax`)


[code language="csharp" title="WebApiConfig"]
public class MessageService : IMessageService
public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            //......
        }
    }
[/code]

At this stage, a `GET` request aimed at the resouce `/api/message` will result in a `500` error since Web API obviously has no concept of how to inject the `IMessageService` into the api controller's constructor.

## Installing required Ninject packages

We are now in a position to wire up `Ninject` within our solution. Bear in mind that there are some supplementary packages required based on [how you choose to host your API](https://github.com/ninject/Ninject.Web.WebApi/wiki). Since we are using IIS hosting, we will grab the following packages as listed in the [Ninject docs](https://github.com/ninject/Ninject.Web.WebApi/wiki/Setting-up-an-mvc-webapi-application).

Ensure you have the following Nuget packages installed:
* `Ninject`
* `Ninject.Web.WebApi.WebHost` (**NOTE:** this package was not listed as required in the [documentation](https://github.com/ninject/Ninject.Web.Common/wiki/Setting-up-an-IIS-hosted-web-application) but has a dependency on `Ninject.Web.Common.WebHost` anyway)

At this point, your list of installed packages should look something like:

```
PM> get-package

Id                                  Versions                                 ProjectName                                                              
--                                  --------                                 -----------                                                              
Microsoft.AspNet.WebApi             {5.2.3}                                  ApiWithNinject                                                           
Microsoft.AspNet.WebApi.Client      {5.2.3}                                  ApiWithNinject                                                           
Microsoft.AspNet.WebApi.Core        {5.2.3}                                  ApiWithNinject                                                           
Microsoft.AspNet.WebApi.WebHost     {5.2.3}                                  ApiWithNinject                                                           
Microsoft.CodeDom.Providers.DotN... {1.0.0}                                  ApiWithNinject                                                           
Microsoft.Net.Compilers             {1.0.0}                                  ApiWithNinject                                                           
Microsoft.Web.Infrastructure        {1.0.0.0}                                ApiWithNinject                                                           
Newtonsoft.Json                     {6.0.4}                                  ApiWithNinject                                                           
Ninject                             {3.2.2.0}                                ApiWithNinject                                                           
Ninject.Web.Common                  {3.2.0.0}                                ApiWithNinject                                                           
Ninject.Web.Common.WebHost          {3.2.3.0}                                ApiWithNinject                                                           
Ninject.Web.WebApi                  {3.2.0.0}                                ApiWithNinject                                                           
Ninject.Web.WebApi.WebHost          {3.2.4.0}                                ApiWithNinject                                                           
WebActivatorEx                      {2.0}                                    ApiWithNinject   
```
## Wiring up Ninject for DI
Having installed `Ninject.Web.Common.WebHost` will have introduced the templated `NinjectWebCommon` class within your `App_Start` folder. Aside from configuring a `BootStrapper` etc, this is where the framework provides an extension point for you to specify your Ninject bindings etc. 

The template generates a `RegisterServices` method. This is where we will add our bindings to allow injecting the messaging service.

[code language="csharp" title="NinjectWebCommon"]
public static class NinjectWebCommon 
    {
        //Truncated for brevity...
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IMessageService>().To<MessageService>();
        }        
    }       
[/code]

At this point, I found that trying to run the project resulted in the following exception being thrown. To date, I am still unsure why this is being thrown when using ninject right out the box.

```
Ninject.ActivationException was unhandled by user code
  HResult=-2146233088
  Message=Error activating HttpConfiguration
More than one matching bindings are available.
Matching bindings:
  1) binding from HttpConfiguration to method
  2) binding from HttpConfiguration to method
Activation path:
  1) Request for HttpConfiguration
 ```
A simple fix for this was to simply instruct the kernel to `rebind` the instance of the `HttpConfiguration` used. Going back to the `NinjectWebCommon` class, I made the following change in the `CreateKernel` method:

[code language="csharp" title="NinjectWebCommon"]
public static class NinjectWebCommon 
    {
    	//Truncated for brevity...


        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();

            //Instruct the Kernel to rebind the HttpConfiguration to the default config instance provided from the GlobalConfiguration
            kernel.Rebind<HttpConfiguration>().ToMethod(context => GlobalConfiguration.Configuration);
            
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }    
    }       
[/code]

At this point, all my code compiles just fine, the application is successfully hosted within IIS (express) and my resource requests work just fine

```
HTTP/1.1 200 OK
Cache-Control: no-cache
Pragma: no-cache
Content-Type: application/json; charset=utf-8
Expires: -1
Server: Microsoft-IIS/10.0
X-AspNet-Version: 4.0.30319
X-SourceFiles: =?UTF-8?B?UzpcV29ya3NwYWNlXGdpdGh1YlxwaXJhbnN3b3JsZFx1c2luZy1uaW5qZWN0LXdpdGgtd2ViLWFwaS0yXHNyY1xBcGlXaXRoTmluamVjdFxhcGlcbWVzc2FnZQ==?=
X-Powered-By: ASP.NET
Date: Sun, 20 Sep 2015 09:38:05 GMT
Content-Length: 32

"Hello from the Message Service"
```

Hope this saves you from having to go through the same pains that I did. Feel free to [grab my code from Github](https://github.com/pirahawk/piransworld/tree/master/using-ninject-with-web-api-2/src)
