using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_C_Sharp_ConsoleApp.csharp_fundamentals_1
{
    delegate void MyDel1();
    delegate int MyDel2(int x);
    internal class AnonymousMethods
    {
        // Anonymous methods are a way to define inline methods without giving them a name.
        // They are often used in situations where you need to pass a method as an argument to another method, such as with delegates or events.
        // Anonymous methods can be defined using the delegate keyword or using lambda expressions.       
        // Method can be invoked directly or can be assigned to a delegate and then invoked through the delegate.

        

        static void Main(string[] args)
        {
            MyDel1 del1 = delegate ()
            {
                Console.WriteLine("This is an anonymous method without parameters.");
            };
            del1();

            MyDel2 del2 = delegate (int x)
            {
                Console.WriteLine("This is an anonymous method with a parameter: " + x);
                return x;
            };
            int result = del2(10);
            Console.WriteLine("The result is: " + result);

            Action<string> del3 = delegate (string message)
            {
                Console.WriteLine("Message: " + message);
            };
            del3("Hello, world!");
        }

    }
}
