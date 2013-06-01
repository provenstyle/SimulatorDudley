using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;

namespace QueueSimulator.Tests
{
    [TestClass]
    public class MessageProcessorTests
    {
        
        [TestMethod]
        public void Processor_Calls_Run_On_A_Message()
        {
            // Arrange
            bool runCalled = false;
            var message = new Message(() => runCalled = true);
            MessageQueue messageQueue = new MessageQueue();
            messageQueue.Add(message);
            var processor = new MessageProcessor(messageQueue);

            // Act
           Task.Factory.StartNew(processor.Start);

            while (processor.Queue.Count > 0) { Thread.Sleep(100); }

            processor.Stop();

            // Assert
            Assert.IsTrue(runCalled);

        }

       [TestMethod]
       public void should_not_run_expired_messages()
       {
          // Arrange
          bool runCalled = false;
          int timeToLive = 100; 
          var message = new Message(() => runCalled = true, timeToLive);
          MessageQueue messageQueue = new MessageQueue();          
          var processor = new MessageProcessor(messageQueue);

          // Act
          Task.Factory.StartNew(processor.Start);
          Thread.Sleep(150);
          messageQueue.Add(message);

          while (processor.Queue.Count > 0) { Thread.Sleep(100); }

          processor.Stop();

          // Assert
          Assert.IsFalse(runCalled);

        

       }
    }
}
