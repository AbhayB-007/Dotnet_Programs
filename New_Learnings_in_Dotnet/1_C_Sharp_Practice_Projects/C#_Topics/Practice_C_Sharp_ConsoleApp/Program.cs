using Practice_C_Sharp_ConsoleApp.csharp_fundamentals_1;
using System.Runtime.InteropServices;

namespace Practice_C_Sharp_ConsoleApp
{
    public class Program
    {
        [DllImport("user32.dll")]
        static extern int MessageBox(IntPtr hWnd, String text, String caption, uint type);

        public static void Print(string args) => Console.WriteLine(args);

        static void Main(string[] args)
        {
            // ---------------------------------------------------
            // datatypes
            //DataTypes.AllDatatypes();                    

            // ---------------------------------------------------
            // working with files
            //WorkingWithFilesInCSharp.WorkingWithFiles();

            // ---------------------------------------------------
            // error handling
            //ErrorHandlingInCSharp.ErrorHandling();

            // ---------------------------------------------------
            // threads and tasks
            //StartThread();
            //StartTask();

           
        }

        static void StartThread()
        {
            Thread thread = new Thread(Threads.DoWork);
            thread.Start();
            Console.WriteLine("Main thread continues...");
        }

        static void StartTask()
        {
            Task task = Task.Run(() => Tasks.DoWork());
            for (int i = 0; i < 50; i++)
            {
                Console.WriteLine($"Main thread is doing work {i}");                
            }           
            task.Wait(); // wait for completion
        }
    }
    class Threads
    {
        public static void DoWork()
        {
            Console.WriteLine("Work started on thread");
            Thread.Sleep(2000);
            Console.WriteLine("Work completed");
        }
    }

    class Tasks
    {
        public static void DoWork()
        {
            Console.WriteLine("Work started using Task");
            Task.Delay(2000).Wait();
            Console.WriteLine("Work completed");
        }
    }

    public class Box<T>
    {
        public T Content { get; set; }

    }
}
