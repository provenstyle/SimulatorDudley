using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

          queueManager = new QueueManager(cars, 20);
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

          before = queueManager.Processors[0].Processing;
          queueManager.Stop();
          Thread.Sleep(500);
          after = queueManager.Processors[0].Processing;

          //Assert
          Assert.IsTrue(before);
          Assert.IsFalse(after);
       }


    }
}
