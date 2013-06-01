using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace QueueSimulator.Tests.Integration
{
    [TestClass]
    public class QueueManagerIntegrationTests
    {
       private List<Car> cars;
       private QueueManager queueManager;
          
       [TestInitialize]
       public void SetUp()
       {
          cars = new List<Car>()
                  {
                     new Car()
                  };
          
          queueManager = new QueueManager(new MessageProcessorFactory());
          queueManager.Init(cars, 20);
          queueManager.Start();
       }

       [TestMethod][Slow]
        public void Queue_Msg_And_Ensure_It_Runs()
        {
          //Arrange
          bool processed = false;
          queueManager.Enqueue(new Message(() => { processed = true; }), cars[0], Priority.High);

          //Act
          while (queueManager.MessageCount > 0) { Thread.Sleep(100); }
          queueManager.Stop();

          //Assert
          Assert.IsTrue(processed);
        }

       [TestMethod]
       [Slow]
       public void Processors_Live_Until_Stopped()
       {
          //Arrange
          bool before;
          bool after;
          bool processed = false;
          queueManager.Enqueue(new Message(() => { processed = true; }), cars[0], Priority.High);

          //Act
          while (queueManager.MessageCount > 0) { Thread.Sleep(100); }

          before = queueManager.GetProcessor(0).Processing;
          queueManager.Stop();
          Thread.Sleep(500);
          after = queueManager.GetProcessor(0).Processing;

          //Assert
          Assert.IsTrue(before);
          Assert.IsFalse(after);
       }

       [TestMethod]
       [Slow]
       public void Can_Survive_Exceptions()
       {
          //Arrange
          bool processed = false;
          queueManager.Enqueue(new Message(() => { throw new Exception("Something went wrong"); }), cars[0], Priority.High);
          queueManager.Enqueue(new Message(() => { processed = true; }), cars[0], Priority.High);

          //Act
          while (queueManager.MessageCount > 0) { Thread.Sleep(100); }
          queueManager.Stop();

          //Assert
          Assert.IsTrue(processed);
       }

       [TestMethod] [Slow]
       public void should_raise_event_on_unhandled_thread_exception()
       {
          // Arrange        
          bool exceptionEventRaised = false;
          int key = -1;

          IMessageProcessorFactory exceptionThrowingMessageProcessorFactory
             = new ExceptionThrowingMessageProcessorFactory();
          queueManager = new QueueManager(exceptionThrowingMessageProcessorFactory);
          queueManager.Init(cars, 1);

          // Act
          queueManager.UnhandledThreadException += x =>
          {
             exceptionEventRaised = true;
             key = x;
          };

          queueManager.Start();

          Thread.Sleep(100);

          // Assert
          Assert.IsTrue(exceptionEventRaised);
          Assert.AreEqual(0, key);
       }
    }
}
