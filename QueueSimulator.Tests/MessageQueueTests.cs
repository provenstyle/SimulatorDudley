using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;

namespace QueueSimulator.Tests
{
    [TestClass]
    public class MessageQueueTests
    {
        private MessageQueue messageQueue;

        [TestInitialize]
        public void Setup()
        {
            messageQueue = new MessageQueue();
        }

        [TestMethod]
        public void Should_Add_Item_To_Queue()
        {
            // Arrange

            // Act
            messageQueue.Add(new Message());

            // Assert
            Assert.AreEqual(messageQueue.Count, 1);
        }

        [TestMethod]
        public void Should_Add_Item_To_Queue_With_Priority()
        {
            // Arrange

            // Act
            messageQueue.Add(new Message(), Priority.High);

            // Assert
            Assert.AreEqual(messageQueue.Count, 1);
            Assert.AreEqual(messageQueue.PriorityCount(Priority.High), 1);
        }

        [TestMethod]
        public void GetQueue_Should_Return_ConcurrentQueue_For_Priority()
        {
            // Arrange

            // Act
           messageQueue.Add(new Message(), Priority.High);

            // Assert
            Assert.AreEqual(1, messageQueue.PriorityCount(Priority.High));
        }

        [TestMethod]
        public void Dequeue_Should_Return_High_Priority_First()
        {
            // Arrange
            messageQueue.Add(new Message(), Priority.Low);
            Message highMsg = new Message();
            messageQueue.Add(highMsg, Priority.High);

            // Act
            var msg = messageQueue.Dequeue();

            // Assert
            Assert.AreSame(highMsg, msg);
        }

    }
}
