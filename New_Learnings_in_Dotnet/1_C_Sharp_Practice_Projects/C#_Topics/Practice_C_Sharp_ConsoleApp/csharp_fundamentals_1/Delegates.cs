using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_C_Sharp_ConsoleApp.csharp_fundamentals_1
{

    delegate void MyDel(int x, int y);
    delegate void MyDelString();

    internal class Delegates
    {
        // Delegates
        //1). It is a reference type variable that holds the reference to a method.
        //2). Used for implementing events and the call-back methods.
        //3). Implicitly derived from the System.Delegate class.
        //4). Delegate holding the reference of multiple methods is called multicast delegate.

        static void Main(string[] args)
        {
            MyDel del1 = new MyDel(TestDelegates.Add);
            del1 += TestDelegates.Multiply;
            MyDelString del2 = new MyDelString(TestDelegates.GetMessage);
            //int result = del1(10, 20);
            //string message = del2();

            Console.WriteLine($"Result: ");
            del1(10, 20);
            Console.Write($"Message: ");
            del2();

        }

    }

    class TestDelegates
    {
        public static void Add(int a, int b)
        {
            Console.WriteLine("Addition: " + (a + b));
        }

        public static void Multiply(int a, int b)
        {
            Console.WriteLine("Multiplication: " + (a * b));
        }

        public static void GetMessage()
        {
            Console.WriteLine("Hello, World!");            
        }

    }




}
