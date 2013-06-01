using System;
using System.Threading;

namespace QueueSimulator
{
   public class MessageProcessor
   {
      private bool _stopSignal = false;
      private readonly MessageQueue _queue;

      public bool Processing { get; private set; }
      public int QueueCount { get { return _queue.Count; } }

      public MessageProcessor(MessageQueue queue)
      {
         _queue = queue;
      }

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
