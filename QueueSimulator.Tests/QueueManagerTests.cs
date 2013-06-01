using System;
using System.Linq;
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
            Assert.AreEqual(1, target.ThreadCount);
        }

        [TestMethod]
        public void AddingOneCarCreatesOneThread()
        {
           // Arrange
           target = new QueueManager(listOfCars, 20);

           // Act
           target.Start();

           // Assert
           Assert.IsTrue(target.ThreadCount == 1);
        }

       [TestMethod]
       public void should_create_one_queue_per_thread()
       {
          // Arrange
          target = new QueueManager(listOfCars,20);


          // Act
          target.Start();

          // Assert
          Assert.AreEqual(target.QueueCount, 1);

       }

       [TestMethod]
       public void should_specify_number_of_cars_per_thread()
       {
          // Arrange
          target = new QueueManager(listOfCars, 1);

          // Act
          target.Start();

          // Assert
          Assert.AreEqual(2, target.ProcessorCount);
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
          Assert.AreEqual(3, target.ProcessorCount);
       }

       [TestMethod]
       public void should_not_create_new_processor_if_there_is_room_on_existing()
       {
          // Arrange
          target = new QueueManager(listOfCars, 5);

          // Act
          target.Start();
          target.AddCar(new Car());

          // Assert
          Assert.AreEqual(1, target.ProcessorCount);
       }

       [TestMethod]
       public void should_track_what_cars_are_on_a_thread()
       {
          // Arrange
          target = new QueueManager(listOfCars, 1);

          // Act
          target.Start();
          var trackedCars = target.GetCarsOnThread(1);

          // Assert
          Assert.AreEqual(listOfCars[1], trackedCars.ToList()[0]);          
       }

       
    }

}