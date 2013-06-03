namespace QueueSimulator
{
   public class MessageProcessorFactoryWithException: IMessageProcessorFactory
   {
      public IMessageProcessor Create(MessageQueue queue)
      {
         return new MessageProcessorWithException(queue);         
      }

      public void Release(IMessageProcessor messageProcessor)
      {
         
      }
   }
}
