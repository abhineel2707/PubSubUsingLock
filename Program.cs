using System;
using System.Threading;

namespace PubSubExample
{
    class Program
    {
        private static readonly object _lock = new object();
        const int BUFFER_CAPACITY = 10;
        const int TIMEOUT_IN_MILLISECONDS = 1000;
        static void Main(string[] args)
        {
            BlockingQueue<int> blockingQueue = new BlockingQueue<int>(5);
            blockingQueue.Write(1);
            blockingQueue.Write(2);
            blockingQueue.Write(3);
            blockingQueue.Write(4);
            Thread producerThread = new Thread(() =>
            {
                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine($"Inside producer thread when i = {i}");
                    ProducerThread(blockingQueue, Program.TIMEOUT_IN_MILLISECONDS);
                }
            });
            Thread consumerThread = new Thread(() =>
            {
                // for (int i = 0; i < 3; i++)
                // {
                Console.WriteLine($"Inside consumer thread when i = {0}");
                ConsumerThread(blockingQueue, Program.TIMEOUT_IN_MILLISECONDS);
                // }
            });
            producerThread.Start();
            consumerThread.Start();
            producerThread.Join();
            consumerThread.Join();
        }
        static void ProducerThread(BlockingQueue<int> blockingQueue, int timeOutInMilliseconds)
        {
            lock (_lock)
            {
                while (blockingQueue.IsFull)
                {
                    // lock is released and we are waiting
                    Monitor.Wait(_lock);
                }

                // After wait is completed we will reacquire the lock
                int item = new Random(10).Next(100, 1000);
                Console.WriteLine($"Inside producer thread. Writing {item} to the buffer queue.");
                Console.WriteLine();
                blockingQueue.Write(item);
            }
        }
        static void ConsumerThread(BlockingQueue<int> blockingQueue, int timeOutInMilliseconds)
        {
            lock (_lock)
            {
                while (!blockingQueue.IsFull)
                {
                    Monitor.Pulse(_lock);
                }
                int item = blockingQueue.Read();
                Console.WriteLine($"Inside consumer thread.");
                Console.WriteLine($"Read {item} from buffer queue");
                Console.WriteLine();
            }
        }
    }
}
