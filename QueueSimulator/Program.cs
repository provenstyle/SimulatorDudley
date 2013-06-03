using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QueueSimulator
{
   class Program
   {
      private static QueueManager queueManager;
      private static List<Car> cars = new List<Car>();
      private static int carsInFleet = 10;
      private static int carsPerProcessor = 10;

      static void Main(string[] args)
      {
         for (int i = 0; i < carsInFleet; i++)
         {
            cars.Add(new Car(){Address = i.ToString()});
         }

         var messageProcessorFactory = new MessageProcessorFactoryWithException();
         queueManager = new QueueManager(messageProcessorFactory);
         queueManager.Init(cars, carsPerProcessor);
         queueManager.Start();

         StartProducers();

         Console.WriteLine("Press any key to exit");
         Console.ReadKey();
      }

      private static void StartProducers()
      {
         LongRunningLowPriorityMessages();
         HighPriorityMessages();
      }

      private static void LongRunningLowPriorityMessages()
      {
         int messagesToAdd = 10;
         foreach (var car in cars)
         {
            for (int i = 0; i < messagesToAdd; i++)
            {
               var message = new Message(() =>
               {
                  Thread.Sleep(100);
                  Console.WriteLine("Car Address: {0}, {1}",
                                    car.Address,
                                    "message processed");
               });
               queueManager.Enqueue(message, car);
            }  
         }
       }

      private static void HighPriorityMessages()
      {
         var timer = new System.Timers.Timer(1000);
         timer.Elapsed += (sender, args) =>
         {
            foreach (var car in cars)
            {
               var message = new Message(() =>
               {
                  Console.WriteLine("Car Address: {0}, {1}",
                                    car.Address,
                                    "High priority message");
               });
               queueManager.Enqueue(message, cars[0], Priority.High);   
            }
         };
         timer.Start();
      }
   }
}