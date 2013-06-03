using System.Collections.Generic;
using System.Threading;

namespace QueueSimulator
{
   public class ManagedGroup
   {
      public int id;
      public Thread Thread { get; set; }
      public MessageQueue MessageQueue { get; set; }
      public IMessageProcessor MessageProcessor { get; set; }
      public List<Car> Cars { get; set; }
      public int ExceptionCount { get; set; }
   }
}