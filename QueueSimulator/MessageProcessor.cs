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

        public MessageProcessor(MessageQueue queue)
        {
            this.Queue = queue;
        }

        public MessageQueue Queue { get; private set; }

        public void Start()
        {
            while (Queue.Count > 0 || stopSignal)
            {
                var msg = Queue.Dequeue();
                msg.Run();
            }
        }

        public void Stop()
        {
            stopSignal = true;
        }
    }
}
