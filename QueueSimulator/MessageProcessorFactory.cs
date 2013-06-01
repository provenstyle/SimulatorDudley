namespace QueueSimulator
{
   public class MessageProcessorFactory: IMessageProcessorFactory
   {
      public IMessageProcessor Create(MessageQueue queue)
      {
         return new MessageProcessor(queue);
      }

      public void Release(IMessageProcessor messageProcessor)
      {
         
      }
   }
}
