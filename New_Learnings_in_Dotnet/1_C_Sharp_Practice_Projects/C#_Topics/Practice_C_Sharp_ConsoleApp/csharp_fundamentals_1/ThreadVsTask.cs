using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_C_Sharp_ConsoleApp.csharp_fundamentals_1
{
    public class Threads
    {
        public static void DoWork()
        {
            Console.WriteLine($"Work started on thread Id : {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(2000);
            Console.WriteLine($"Work completed on thread Id : {Thread.CurrentThread.ManagedThreadId}");
        }
    }

    public class Tasks
    {
        public static void DoWork()
        {
            Console.WriteLine($"Task running on thread ID: {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(1000); // Simulate work
            Console.WriteLine($"Task done on thread ID: {Thread.CurrentThread.ManagedThreadId}");
        }
    }

    public class ThreadVsTask
    {
        static void StartThread()
        {
            Thread thread = new Thread(Threads.DoWork);
            thread.IsBackground = true; // Dies when main program exits
            thread.Start();
            thread.Join(); // Wait for it to finish
            Console.WriteLine("Main thread continues...");
        }

        static async Task StartTask()
        {
            Task task = Task.Run(() => Tasks.DoWork());
            await task; // Wait for the task to complete           
            Console.WriteLine("Main done.");
        }

        static async Task RunningMultipleTasksInParallel()
        {
            Task<string> t1 = Task.Run(() => { Tasks.DoWork(); return $"Task thread ID: {Thread.CurrentThread.ManagedThreadId}"; });
            Task<string> t2 = Task.Run(() => { Tasks.DoWork(); return $"Task thread ID: {Thread.CurrentThread.ManagedThreadId}"; });
            Task<string> t3 = Task.Run(() => { Tasks.DoWork(); return $"Task thread ID: {Thread.CurrentThread.ManagedThreadId}"; });

            string[] results = await Task.WhenAll(t1, t2, t3);

            // Output order: Task 1, Task 2, Task 3 (WhenAll preserves order)
            Console.WriteLine("\n" + string.Join(" > ", results));
            Console.WriteLine("All tasks completed.");
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Comparing Threads and Tasks:");
            Console.WriteLine("Threads are lower-level and provide more control, but require more management.");
            Console.WriteLine("Tasks are higher-level, easier to use, and integrate well with async/await.\n\n");

            // ---------------------------------------------------
            // threads vs tasks
            //StartThread();
            //await StartTask();
            //await RunningMultipleTasksInParallel();

        }
    }
}
