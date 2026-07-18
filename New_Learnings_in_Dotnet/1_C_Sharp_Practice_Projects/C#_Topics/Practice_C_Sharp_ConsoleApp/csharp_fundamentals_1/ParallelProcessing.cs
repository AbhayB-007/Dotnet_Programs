using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Practice_C_Sharp_ConsoleApp.csharp_fundamentals_1
{
    //[ShortRunJob]
    //[MemoryDiagnoser]
    //[Orderer(SummaryOrderPolicy.FastestToSlowest)]
    //[RankColumn(NumeralSystem.Arabic)]
    public class ParallelProcessing
    {
        int[] arr = Enumerable.Range(0, 10).ToArray();

        static async Task Main(string[] args)
        {
            // Parallel processing is a technique that allows multiple tasks or operations to be executed concurrently, taking advantage of multiple CPU cores or processors. It can significantly improve the performance of applications that involve computationally intensive tasks or large datasets.
            // In C#, you can achieve parallel processing using the Task Parallel Library (TPL) and the Parallel class. The TPL provides a set of APIs that make it easier to write parallel code without having to deal with low-level threading details.

            // Example 1: Using simple for loop (sequential processing)
            //new ParallelProcessing().Array();

            // Example 2: Using Parallel.For
            //new ParallelProcessing().ParallelFor();

            // Example 3: Using Parallel.ForEach
            new ParallelProcessing().ParallelForEach();

            // Example 4: Using Task.WhenAll for asynchronous operations
            //await new ParallelProcessing().TaskWhenAll();

            // To run benchmarks in debug mode
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());
        }

        public void Array()
        {
            for (int i = 0; i < arr.Length; i++)
            {
                Console.WriteLine($"Processing array item {arr[i]} on thread {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(500); // Simulate work
            }
        }

        [Benchmark]
        public void ParallelFor()
        {            
            Parallel.For(0, arr.Length, i =>
            {
                Console.WriteLine($"Processing ParralelFor item {arr[i]} on thread {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(500); // Simulate work
            });
        }

        [Benchmark]
        public void ParallelForEach()
        {
            List<int> numbers = Enumerable.Range(1, 10).ToList();
            Parallel.ForEach(arr, number =>
            {
                Console.WriteLine($"Processing number {number} on thread {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(500); // Simulate work
            });
        }

        [Benchmark]
        public async Task TaskWhenAll()
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 5; i++)
            {
                int taskId = i; // Capture the loop variable
                tasks.Add(Task.Run(() =>
                {
                    Console.WriteLine($"Task {taskId} running on thread {Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(1000); // Simulate work
                    Console.WriteLine($"Task {taskId} completed on thread {Thread.CurrentThread.ManagedThreadId}");
                }));
            }
            await Task.WhenAll(tasks.ToArray());
        }

    }
}
