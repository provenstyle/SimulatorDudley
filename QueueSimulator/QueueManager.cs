using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace QueueSimulator
{
   public class QueueManager : IDisposable
   {
      private readonly Dictionary<int, ManagedGroup> _groups;
      private readonly Dictionary<Car, MessageQueue> _queueMap;
      private readonly IMessageProcessorFactory _messageProcessorFactory;

      private IEnumerable<Car> _cars;
      private int _carsPerProcessor;

      public QueueManager(IMessageProcessorFactory messageProcessorFactory)
      {
         _messageProcessorFactory = messageProcessorFactory;
         _queueMap = new Dictionary<Car, MessageQueue>();
         _groups = new Dictionary<int, ManagedGroup>();

         UnhandledThreadException += i =>
            {
               var group = _groups[i];
               group.ExceptionCount++;
               RebuildManagedGroup(group);
            };
      }

      public event Action<int> UnhandledThreadException;
      public ManagedGroup GetManagedGroup(int key) { return _groups[key]; }
      public int ThreadCount { get { return _groups.Count(); } }
      public int QueueCount { get { return _groups.Count; } }
      public int ProcessorCount { get { return _groups.Count; } }
      public int MessageCount { get { return _groups.Sum(x => x.Value.MessageQueue.Count); } }

      public void Init(IEnumerable<Car> cars, int carsPerProcessor)
      {
         _cars = cars;
         _carsPerProcessor = carsPerProcessor;
      }

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
         CreateGroup(new List<Car> { car });
      }

      public void RemoveCar(Car car)
      {
         _queueMap.Remove(car);
         
         
         foreach (var group in _groups)
         {
            Car carToRemove = null;
            foreach (var trackedCar in group.Value.Cars)
            {
               if (trackedCar == car)
               {
                  carToRemove = trackedCar;
               }
            }
            if (carToRemove != null)
            {
               group.Value.Cars.Remove(carToRemove);
            }
         }

         CleanupUnNeededGroups();
      }

      private void CleanupUnNeededGroups()
      {
         var managedGroupsToRemove = new List<int>();

         foreach (var group in _groups)
         {
            if (group.Value.Cars.Count == 0)
            {
               group.Value.MessageProcessor.Stop();
               group.Value.Thread.Abort();
               managedGroupsToRemove.Add(group.Key);
            }            
         }

         foreach (var key in managedGroupsToRemove)
         {
            _groups.Remove(key);
         }
      }

      public IMessageProcessor GetProcessor(int key)
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

      private ManagedGroup CreateGroup(List<Car> cars)
      {
         var id = _groups.Count;
         var queue = new MessageQueue();
         var processor = _messageProcessorFactory.Create(queue);
         var thread = CreateThread(processor, id);

         var group = new ManagedGroup
         {
            id = id,
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

      private Thread CreateThread(IMessageProcessor processor, int id)
      {
         return new Thread(() =>
            {
               try
               {
                  processor.Start();
               }
               catch (ThreadAbortException abortException)
               {
                  Logger.Debug("Thread was shutdown.");
               }
               catch (Exception ex)
               {
                  Logger.Error(ex, "Unhandled exception in message processor.");
                  UnhandledThreadException(id);
               }
            });
      }

      private void RebuildManagedGroup(ManagedGroup group)
      {
         var queue = new MessageQueue();
         var processor = _messageProcessorFactory.Create(queue);
         var thread = CreateThread(processor, group.id);

         group.MessageQueue = queue;
         group.MessageProcessor = processor;
         group.Thread = thread;

         //remove old mapping
         foreach (var car in group.Cars)
         {
            _queueMap.Remove(car);
         }

         //add new mapping
         foreach (var car in group.Cars)
         {
            _queueMap.Add(car, queue);
         }
         thread.Start();
      }

      private void StartThreads()
      {
         foreach (var group in _groups)
         {
            group.Value.Thread.Start();
         }
      }

      public void Dispose()
      {
         foreach (var group in _groups)
         {
            _messageProcessorFactory
               .Release(group.Value.MessageProcessor);
         }
      }
   }
}
