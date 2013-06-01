using System;

namespace QueueSimulator.Tests
{
   public class ExceptionThrowingMessageProcessorFactory : IMessageProcessorFactory
   {
      public IMessageProcessor Create(MessageQueue queue)
      {
         return new ExceptionThrowingMessageProcessor();
      }

      public void Release(IMessageProcessor messageProcessor)
      {
         
      }
   }
}