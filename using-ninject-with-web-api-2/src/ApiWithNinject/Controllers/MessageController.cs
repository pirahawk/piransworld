using ApiWithNinject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiWithNinject.Controllers
{
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
}
