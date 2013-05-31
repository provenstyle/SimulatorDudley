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
        public Message()
        {
            this.runAction = () => { };
        }

        public Message(Action runAction)
        {
            this.runAction = runAction;
        }

        private readonly Action runAction;

        public void Run()
        {
            runAction();
        }
    }
}
