using System;
using System.Threading;

namespace QueueSimulator.Tests
{
   public class TestExceptionThrowingMessageProcessor : IMessageProcessor
   {
      public bool Processing { get; private set; }
      public int QueueCount { get; private set; }
      public void Start()
      {
         Thread.Sleep(10);
         throw new Exception("Unhandled thread exception.");
      }

      public void Stop()
      {
         
      }
   }
}