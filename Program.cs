using System.Threading.Tasks;
using System;
using System.Threading;

namespace Semaphore
{
    class Program
    {
        //TODO:信号量
        //1、是一种计数的互斥锁定
        //2、限定同时访问保护资源的线程个数
        static void Main(string[] args)
        {
            int taskCount=6;
            int semaphoreCount=3;
            var semaphore =new SemaphoreSlim(semaphoreCount,semaphoreCount);
            var tasks=new Task[taskCount];
            for(int i=0;i<taskCount;i++)
            {
                tasks[i]=Task.Run(()=>TaskMain(semaphore));
            }

            Task.WaitAll(tasks);
            Console.WriteLine("All tasks finished");

            Console.WriteLine("Hello World!");
        }

        public static void TaskMain(SemaphoreSlim semaphore)
        {
            bool isCompleted=false;
            while(!isCompleted)
            {
                if(semaphore.Wait(600))
                {
                    try
                    {
                        Console.WriteLine($"Task {Task.CurrentId} locks the semaphore");
                        Task.Delay(2000).Wait();
                    }
                    finally
                    {
                        Console.WriteLine($"Task {Task.CurrentId} releases the semaphore");
                        semaphore.Release();
                        isCompleted=true;
                    }
                }
                else
                {
                    Console.WriteLine($"Timeout for task {Task.CurrentId}; wait again");
                }
            }
        }
    }
}
