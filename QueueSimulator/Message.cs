using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QueueSimulator
{
    public class Message
    {
       private int _timeToLive;
       private DateTime _created;       

        public Message()
           :this(() => {})
        {
        }

        public Message(Action runAction, int timeToLive = -1)
        {
            this.runAction = runAction;
           _timeToLive = timeToLive;
           _created = DateTime.Now;
        }

        private readonly Action runAction;

        public void Run()
        {
           if (Expired()) return;
            runAction();
        }

        public bool Expired()
        {
           if (_timeToLive == -1) return false;
           return DateTime.Now > _created.AddMilliseconds(_timeToLive);
        }
    }
}
