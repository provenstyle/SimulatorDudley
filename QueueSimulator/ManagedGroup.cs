using System.Collections.Generic;
using System.Threading;

namespace QueueSimulator
{
   public class ManagedGroup
   {
      public Thread Thread { get; set; }
      public MessageQueue MessageQueue { get; set; }
      public IMessageProcessor MessageProcessor { get; set; }
      public List<Car> Cars { get; set; }
   }
}