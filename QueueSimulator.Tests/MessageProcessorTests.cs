using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;
using Moq;

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
            processor.Start();

            while (processor.Queue.Count > 0) { Thread.Sleep(100); }

            processor.Stop();

            // Assert
            Assert.IsTrue(runCalled);

        }
    }
}
