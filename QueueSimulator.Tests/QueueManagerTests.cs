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

        }
        [TestMethod]
        public void Start_Should_Create_One_Thread_Per_20_Cars()
        {
            // Arrange
           target = new QueueManager(listOfCars,20);

            // Act
            target.Start();

            // Assert
            Assert.AreEqual(1, target.Threads.Count);
        }

        [TestMethod]
        public void AddingOneCarCreatesOneThread()
        {
           // Arrange
           target = new QueueManager(listOfCars, 20);

           // Act
           target.Start();

           // Assert
           Assert.IsTrue(target.Threads.Count == 1);
        }

       [TestMethod]
       public void should_create_one_queue_per_thread()
       {
          // Arrange
          target = new QueueManager(listOfCars,20);


          // Act
          target.Start();

          // Assert
          Assert.AreEqual(target.Queues.Count, 1);

       }

       [TestMethod]
       public void should_specify_number_of_cars_per_thread()
       {
          // Arrange
          target = new QueueManager(listOfCars, 1);

          // Act
          target.Start();

          // Assert
          Assert.AreEqual(2, target.Processors.Count);
       }

       [TestMethod]
       public void should_add_new_cars_to_the_manager_after_it_is_started()
       {
          // Arrange
          target = new QueueManager(listOfCars, 1);

          // Act
          target.Start();
          target.AddCar(new Car());

          // Assert
          Assert.AreEqual(3, target.Processors.Count);
       }

       
    }

}