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

        public QueueManager(IEnumerable<Car> cars)
        {
            this.cars = cars;
            Threads = new List<Thread>();
        }
                
        public List<Thread> Threads { get; set; }


        public void Start()
        {
            var allCars = new List<Car>(cars);
            IEnumerable<Car> set;
            while (true)
            {
                set = allCars.Take(20).ToList();
                foreach (Car car in set)
                {
                    allCars.Remove(car);
                }
                Threads.Add(new Thread(() => { Thread.Sleep(1000); }));
                if (set.Count() < 20) break;
            }

            StartThreads();
        }

        public void StartThreads()
        {
            foreach (var thread in Threads)
            {
                thread.Start();
            }
        }
    }
}
