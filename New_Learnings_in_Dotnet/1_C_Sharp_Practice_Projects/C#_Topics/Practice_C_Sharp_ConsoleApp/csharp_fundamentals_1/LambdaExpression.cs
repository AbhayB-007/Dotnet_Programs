using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_C_Sharp_ConsoleApp.csharp_fundamentals_1
{
    // custom delegates
    delegate T MyCustomDel<T>(T a);
    delegate void MyCustomAction(string message);

    internal class LambdaExpression
    {
        // custom delegates
        delegate void MyAction(string message);

        static void Main(string[] args)
        {
            // Lambda expressions are a concise way to represent anonymous methods using a more compact syntax. They are often used in LINQ queries and with delegates or events.
            // A lambda expression uses the => operator to separate the parameters from the body of the expression. The left side of the operator specifies the parameters, and the right side specifies the expression or statement block that defines the method's behavior.

            Func<int, int> square = x => x * x;
            Console.WriteLine("Square of 5: " + square(5));

            Func<int, int, float> add = (x, y) => x + y;
            Console.WriteLine("Sum of 3 and 4: " + add(3, 4));

            //Func<int, int, float> addDelegate = delegate (int x, int y) { return x + y; };  
            //Console.WriteLine("Sum of 3 and 4: " + addDelegate(3, 4));

            Action<string> greet = name => Console.WriteLine("Hello, " + name + "!");
            greet("Abhay");

            // custom delegates
            MyAction customGreet = message => {
                Console.Write("Enter your Name: ");
                string? name = Console.ReadLine();
                if (string.IsNullOrEmpty(name))
                {
                    name = "Guest";
                }
                message += " " + name;
                Console.WriteLine("Custom Greeting: " + message);                                   
            };
            customGreet("Welcome to C# programming");

            MyCustomAction msg = message => Console.WriteLine("Custom Action: " + message);
            msg("This is a custom action using a lambda expression.");

            MyCustomDel<int> doubleValue = x => x * 2;
            Console.WriteLine("Double of 5: " + doubleValue(5));


        }
    }
}
