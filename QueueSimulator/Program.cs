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
      private static List<Car> cars;

      static void Main(string[] args)
      {
         cars = new List<Car> { new Car() };
         queueManager = new QueueManager(cars, 20);
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
          for (int i = 0; i < 100; i++)
          {
             var message = new Message(() =>
                           {
                              Thread.Sleep(100);
                              Console.WriteLine("Thread: {0}, {1}", Thread.CurrentContext.ContextID,
                                                "message processed");
                           });
             queueManager.Enqueue(message, cars[0]);
          }
       }

      private static void HighPriorityMessages()
      {
         var timer = new System.Timers.Timer(1000);
         timer.Elapsed += (sender, args) =>
         {
            var message = new Message(() =>
            {
               Console.WriteLine("Thread: {0}, {1}",
                                 Thread.CurrentContext.ContextID,
                                 "High priority message");
            });
            queueManager.Enqueue(message, cars[0], Priority.High);
         };
         timer.Start();
      }
   }
}