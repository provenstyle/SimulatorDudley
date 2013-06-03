using System;
using System.Threading;

namespace QueueSimulator.Tests
{
   public class ExceptionThrowingMessageProcessorFactory : IMessageProcessorFactory
   {
      public IMessageProcessor Create(MessageQueue queue)
      {
         
         return new TestExceptionThrowingMessageProcessor();
      }

      public void Release(IMessageProcessor messageProcessor)
      {
         
      }
   }
}