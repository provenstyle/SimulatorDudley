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
       private readonly Dictionary<Car, MessageQueue> queueMap;

        public QueueManager(IEnumerable<Car> cars)
        {
            this.cars = cars;
            Threads = new List<Thread>();
           Queues = new List<MessageQueue>();
           queueMap = new Dictionary<Car, MessageQueue>();
        }
                
        public List<Thread> Threads { get; set; }
        public List<MessageQueue> Queues { get; set; }
       public int MessageCount
       {
          get { return Queues.Sum(x => x.Count); }
       }

        public void Start()
        {
            var allCars = new List<Car>(cars);
            IEnumerable<Car> set;
            while (true)
            {
                set = allCars.Take(20).ToList();
               var queue = new MessageQueue();
               var processor = new MessageProcessor(queue);
                foreach (Car car in set)
                {
                    allCars.Remove(car);
                    queueMap.Add(car, queue);
                }

               var thread = new Thread(() => { processor.Start(); });
                Threads.Add(thread);
                Queues.Add(queue);
                if (set.Count() < 20) break;
            }

            StartThreads();
        }

       public void Enqueue(Message message, Car car, Priority priority = Priority.Low)
       {
          MessageQueue queue;
          if (queueMap.TryGetValue(car, out queue))
          {
             queue.Add(message, priority);
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
}
