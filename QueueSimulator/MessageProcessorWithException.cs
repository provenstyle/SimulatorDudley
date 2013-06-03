using System;
using System.Threading;

namespace QueueSimulator
{
   public class MessageProcessorWithException : IMessageProcessor
   {
      private bool _stopSignal = false;
      private readonly MessageQueue _queue;

      public bool Processing { get; private set; }
      public int QueueCount { get { return _queue.Count; } }

      public MessageProcessorWithException(MessageQueue queue)
      {
         _queue = queue;
      }


      private int count = 0;
      public void Start()
      {
         Processing = true;
         do
         {
            while (_queue.Count > 0)
            {
               try
               {
                  var msg = _queue.Dequeue();
                  msg.Run();
               }
               catch (Exception ex)
               {
                  Logger.Error(ex, "Error processing message.");
               }
            }
            count++;

            if (count == 5)
            {
               throw new Exception("intentional unhandled exception");
            }
            Thread.Sleep(250);
         } while (!_stopSignal);
         Processing = false;
      }

      public void Stop()
      {
         _stopSignal = true;
      }
   }
}
