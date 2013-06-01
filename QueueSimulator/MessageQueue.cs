using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace QueueSimulator
{
   public class MessageQueue
   {
      private readonly Dictionary<Priority, ConcurrentQueue<Message>> queues;
      public MessageQueue()
      {
         queues = new Dictionary<Priority, ConcurrentQueue<Message>>();
         foreach (Priority val in Enum.GetValues(typeof(Priority)))
         {
            queues.Add(val, new ConcurrentQueue<Message>());
         }
      }

      public int Count { get { return queues.Values.Sum(e => e.Count); } }

      public int PriorityCount(Priority priority)
      {
         return queues[priority].Count;
      }

      public void Add(Message message, Priority priority = Priority.Low)
      {
         queues[priority].Enqueue(message);
      }

      public Message Dequeue()
      {
         foreach (var pair in queues.OrderBy(e => e.Key))
         {
            Message msg;
            if (pair.Value.TryDequeue(out msg))
               return msg;
         }
         return null;
      }

   }
}
