using System;
using System.Collections.Generic;
using System.Text;

namespace Practice_C_Sharp_ConsoleApp.csharp_fundamentals_1
{
    public class CancellationToken
    {
        public static void Main(string[] args)
        {
            // Create a CancellationTokenSource
            var cts = new System.Threading.CancellationTokenSource();

            // Start a task that will be canceled after 2 seconds
            var task = System.Threading.Tasks.Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    if (cts.Token.IsCancellationRequested)
                    {
                        Console.WriteLine("Task was canceled.");
                        return;
                    }
                    Console.WriteLine($"Working... {i}");
                    System.Threading.Thread.Sleep(500);
                }
            }, cts.Token);

            // Cancel the task after 2 seconds
            System.Threading.Thread.Sleep(2000);
            cts.Cancel();

            // Wait for the task to complete
            try
            {
                task.Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var inner in ex.InnerExceptions)
                {
                    Console.WriteLine(inner.Message);
                }
            }
            Console.WriteLine("Main method complete.");            
        }
    }
}
