using System.Web;

namespace MyCustomHandler.Infrastructure
{
    public class PrintIdHttpHandler : IHttpHandler
    {
        private readonly int _id;

        public PrintIdHttpHandler(int id)
        {
            _id = id;
        }

        public void ProcessRequest(HttpContext context)
        {
            var message = string.Format("Recieved Id:{0}", _id);
            context.Response.Write(message);
        }

        public bool IsReusable { get; private set; }
    }
}