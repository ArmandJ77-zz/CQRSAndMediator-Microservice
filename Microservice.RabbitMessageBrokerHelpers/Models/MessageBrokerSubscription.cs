using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.RabbitMessageBrokerHelpers.Models
{
   public class MessageBrokerSubscription
    {
        public string Pool { get; set; }
        public Type Handler { get; set; }
        public Type EventModel { get; set; }
        public string Topic { get; set; }
    }
}
