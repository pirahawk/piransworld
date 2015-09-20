using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiWithNinject.Services
{
    public class MessageService : IMessageService
    {
        public string Message()
        {
            return "Hello from the Message Service";
        }
    }
}