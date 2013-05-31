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
       [TestMethod][Slow]
        public void Queue_Msg_And_Ensure_It_Runs()
        {
          //Arrange
          bool processed = false;
          var cars = new List<Car>()
                        {
                           new Car()
                        };
          var queueManager = new QueueManager(cars);
          queueManager.Start();
          queueManager.Enqueue(new Message(() => { processed = true; }), cars[0], Priority.High);

          while(queueManager.MessageCount > 0){Thread.Sleep(100);}
          
          //Act

          //Assert
          Assert.IsTrue(processed);
          
        }
    }
}
