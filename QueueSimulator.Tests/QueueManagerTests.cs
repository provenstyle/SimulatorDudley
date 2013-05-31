using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;

namespace QueueSimulator.Tests
{
    [TestClass]
    public class QueueManagerTests
    {
        private List<Car> listOfCars;
        private QueueManager target;

        [TestInitialize]
        public void Setup()
        {
            listOfCars = new List<Car>()
                                         {
                                           new Car(),
                                           new Car()
                                         };
            target = new QueueManager(listOfCars);

        }
        [TestMethod]
        public void Start_Should_Create_One_Thread_Per_20_Cars()
        {
            // Arrange

            // Act
            target.Start();

            // Assert
            Assert.AreEqual(1, target.Threads.Count);
        }

        [TestMethod]
        public void AddingOneCarCreatesOneThread()
        {
           // Arrange

           // Act
           target.Start();

           // Assert
           Assert.IsTrue(target.Threads.Count == 1);
        }

        [TestMethod]
        public void Should_Start_Threads_On_Start()
        {
            // Arrange
            

            // Act
            target.Start();

            // Assert
            Assert.AreEqual(ThreadState.Running, target.Threads[0].ThreadState);
        }

    }

}