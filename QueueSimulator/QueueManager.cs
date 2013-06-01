using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace QueueSimulator
{
   public class QueueManager
   {
      private readonly IEnumerable<Car> _cars;
      private readonly Dictionary<int, Group> _groups;
      private readonly Dictionary<Car, MessageQueue> _queueMap;
      private readonly int _carsPerProcessor;

      public QueueManager(IEnumerable<Car> cars, int carsPerProcessor)
      {
         _cars = cars;
         _carsPerProcessor = carsPerProcessor;
         _queueMap = new Dictionary<Car, MessageQueue>();
         _groups = new Dictionary<int, Group>();
      }

      public int ThreadCount { get { return _groups.Count(); } }
      public int QueueCount { get { return _groups.Count; } }      
      public int ProcessorCount { get { return _groups.Count; } }
      public int MessageCount { get { return _groups.Sum(x => x.Value.MessageQueue.Count); } }

      public void Start()
      {
         BalanceCars();
         StartThreads();
      }

      public void Stop()
      {
         foreach (var group in _groups)
         {
            group.Value.MessageProcessor.Stop();
         }
      }

      public void Enqueue(Message message, Car car, Priority priority = Priority.Low)
      {
         MessageQueue queue;
         if (_queueMap.TryGetValue(car, out queue))
         {
            queue.Add(message, priority);
         }
      }

      public void AddCar(Car car)
      {
         foreach (var group in _groups)
         {
            if (group.Value.Cars.Count() < _carsPerProcessor)
            {
               group.Value.Cars.Add(car);
               return;
            }
         }
         CreateGroup(new List<Car> {car});
      }

      public MessageProcessor GetProcessor(int key)
      {
         return _groups[key].MessageProcessor;
      }

      public IEnumerable<Car> GetCarsOnThread(int key)
      {
         return _groups[key].Cars;
      }

      private void BalanceCars()
      {
         var allCars = new List<Car>(_cars);

         while (allCars.Any())
         {
            var set = allCars.Take(_carsPerProcessor).ToList();
            CreateGroup(set);
            foreach (Car car in set)
            {
               allCars.Remove(car);
            }
         }
      }

      private Group CreateGroup(List<Car> cars)
      {
         var id = _groups.Count;
         var queue = new MessageQueue();
         var processor = new MessageProcessor(queue);
         var thread = new Thread(() => { processor.Start(); });

         var group = new Group
         {
            Cars = cars,
            MessageProcessor = processor,
            MessageQueue = queue,
            Thread = thread
         };

         _groups.Add(id, group);

         foreach (var car in cars)
         {
            _queueMap.Add(car, queue);
         }

         return group;
      }

      private void StartThreads()
      {
         foreach (var group in _groups)
         {
            group.Value.Thread.Start();
         }
      }
   }

   public class Group
   {
      public Thread Thread { get; set; }
      public MessageQueue MessageQueue { get; set; }
      public MessageProcessor MessageProcessor { get; set; }
      public List<Car> Cars { get; set; }
   }
}
