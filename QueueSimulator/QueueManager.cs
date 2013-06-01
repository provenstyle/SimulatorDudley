using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QueueSimulator
{
   public class QueueManager
   {
      private readonly IEnumerable<Car> cars;
      private readonly Dictionary<int, List<Car>> GroupedCars;

      private readonly Dictionary<Car, MessageQueue> queueMap;
      private readonly int carsPerProcessor;

      public QueueManager(IEnumerable<Car> cars, int carsPerProcessor)
      {
         this.cars = cars;
         this.carsPerProcessor = carsPerProcessor;
         Threads = new List<Thread>();
         Queues = new List<MessageQueue>();
         Processors = new List<MessageProcessor>();
         queueMap = new Dictionary<Car, MessageQueue>();
         GroupedCars = new Dictionary<int, List<Car>>();
      }

      public List<Thread> Threads { get; set; }
      public List<MessageQueue> Queues { get; set; }
      public List<MessageProcessor> Processors { get; set; }
      public int MessageCount
      {
         get { return Queues.Sum(x => x.Count); }
      }

      public void Start()
      {
         BalanceCars();
         StartThreads();
      }

      public void Stop()
      {
         foreach (var messageProcessor in Processors)
         {
            messageProcessor.Stop();
         }
      }

      public void Enqueue(Message message, Car car, Priority priority = Priority.Low)
      {
         MessageQueue queue;
         if (queueMap.TryGetValue(car, out queue))
         {
            queue.Add(message, priority);
         }
      }

      public void AddCar(Car car)
      {
         foreach (var group in GroupedCars)
         {
            if (group.Value.Count < carsPerProcessor)
               group.Value.Add(car);
         }
      }

      private void BalanceCars()
      {
         var allCars = new List<Car>(cars);
         IEnumerable<Car> set;
         int idCounter = 0;
         while (allCars.Any())
         {
            set = allCars.Take(carsPerProcessor).ToList();
            GroupedCars.Add(idCounter, set.ToList());
            idCounter ++;

            var queue = new MessageQueue();
            Queues.Add(queue);
            var processor = new MessageProcessor(queue);
            Processors.Add(processor);
            var thread = new Thread(() => { processor.Start(); });
            Threads.Add(thread);

            var group = new Group
            {
               Cars = set,
               MessageProcessor = processor,
               MessageQueue = queue,
               Thread = thread
            };

            
            foreach (Car car in set)
            {
               allCars.Remove(car);
               queueMap.Add(car, queue);
            }            
         }
      }


      private void StartThreads()
      {
         foreach (var thread in Threads)
         {
            thread.Start();
         }
      }
   }

   public class Group
   {
      public Thread Thread { get; set; }
      public MessageQueue MessageQueue { get; set; }
      public MessageProcessor MessageProcessor { get; set; }
      public IEnumerable<Car> Cars { get; set; }
   }
}
