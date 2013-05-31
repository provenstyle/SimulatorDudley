using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QueueSimulator
{
    public interface IMessage
    {
        void Run();
    }
}
