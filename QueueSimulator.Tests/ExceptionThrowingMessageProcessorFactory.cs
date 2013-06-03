using System;
using System.Threading;

namespace QueueSimulator.Tests
{
   public class ExceptionThrowingMessageProcessorFactory : IMessageProcessorFactory
   {
      public IMessageProcessor Create(MessageQueue queue)
      {
         Thread.Sleep(10);
         return new TestExceptionThrowingMessageProcessor();
      }

      public void Release(IMessageProcessor messageProcessor)
      {
         
      }
   }
}