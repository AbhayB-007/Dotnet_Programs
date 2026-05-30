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

        static void Main(string[] args)
        {
            MyDel del1 = new MyDel(Add);
            del1 += Multiply;
            MyDelString del2 = new MyDelString(GetMessage);
            //int result = del1(10, 20);
            //string message = del2();

            Console.WriteLine($"Result:- ");
            del1(10, 20);
            Console.Write($"Message: ");
            del2();
        }
    }

    class GenericDelegates
    {
        // Generic Delegates
        //1). Action<T> - Represents a method that takes a parameter and does not return a value.
        //2). Func<T, TResult> - Represents a method that takes a parameter and returns a value.
        //3). Predicate<T> - Represents a method that takes a parameter and returns a boolean value.

        public static void SampleMethod(string name)
        {
            Console.WriteLine("Welcome, " + name);
        }

        public static int Add1(int x, int y)
        {
            x = 1; y = 2;
            return x + y;
        }

        public static bool Login(string uid)
        {
            if (uid == "Abhay")
                return true;
            else
                return false;
        }

        static void Main(string[] args)
        {            
            Action<string> action1 = SampleMethod;
            Func<int, int, int> func1 = Add1;
            Predicate<string> predicate1 = Login;

            action1("Abhay");
            
            int result = func1(10, 20);
            Console.WriteLine("Result: " + result);
            
            bool isLoggedIn = predicate1("Abhay");
            Console.WriteLine("Login Successful: " + isLoggedIn);
        }
    }




}
