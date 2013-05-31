using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QueueSimulator
{
    public class MessageProcessor
    {
        private bool stopSignal = false;

        public bool Processing { get; private set; }
        public MessageProcessor(MessageQueue queue)
        {
            this.Queue = queue;
        }

        public MessageQueue Queue { get; private set; }

        public void Start()
        {
           Processing = true;
           do
           {
              while (Queue.Count > 0 )
              {
                 var msg = Queue.Dequeue();
                 msg.Run();
              }  
              Thread.Sleep(250);
           } while (!stopSignal);
           Processing = false;
        }

        public void Stop()
        {
            stopSignal = true;
        }
    }
}
