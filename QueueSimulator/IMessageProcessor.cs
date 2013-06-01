namespace QueueSimulator
{
   public interface IMessageProcessor
   {
      bool Processing { get; }
      int QueueCount { get; }
      void Start();
      void Stop();
   }

   public interface IMessageProcessorFactory
   {
      IMessageProcessor Create(MessageQueue queue);
      void Release(IMessageProcessor messageProcessor);
   }
}