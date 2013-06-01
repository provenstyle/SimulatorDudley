using System;

namespace QueueSimulator.Tests
{
   public class ExceptionThrowingMessageProcessor : IMessageProcessor
   {
      public bool Processing { get; private set; }
      public int QueueCount { get; private set; }
      public void Start()
      {
         throw new Exception("Unhandled thread exception.");
      }

      public void Stop()
      {
         
      }
   }
}