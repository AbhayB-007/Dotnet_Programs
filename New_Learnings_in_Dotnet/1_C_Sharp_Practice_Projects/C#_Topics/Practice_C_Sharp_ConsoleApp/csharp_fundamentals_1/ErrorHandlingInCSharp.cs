using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_C_Sharp_ConsoleApp.csharp_fundamentals_1
{
    public class ErrorHandlingInCSharp
    {

        public static void ErrorHandling()
        {
            //// ----------------------------------------------------------------
            //// Basic Try-Catch

            //try
            //{
            //    int a = 10;
            //    int b = 0;
            //    int result = a / b;   // Exception here
            //}
            //catch (DivideByZeroException ex)
            //{
            //    Console.WriteLine($"You cannot divide by zero. | error message : {ex.Message} | error stack trace : {ex.StackTrace}\n\n");
            //}

            //// ----------------------------------------------------------------
            //// Multiple Catch Blocks

            //try
            //{
            //    int num = int.Parse("abc");
            //}
            //catch (FormatException ex)
            //{
            //    Console.WriteLine($"Invalid format. | error message : {ex.Message} | error stack trace : {ex.StackTrace}\n\n");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Something went wrong. | error message : {ex.Message} | error stack trace : {ex.StackTrace}\n\n");
            //}

            //// ----------------------------------------------------------------
            //// Finally Block

            //try
            //{
            //    Console.WriteLine("Inside try");
            //    throw new Exception("This is a test exception");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error occurred | error message : {ex.Message} | error stack trace : {ex.StackTrace}\n\n");
            //}
            //finally
            //{
            //    Console.WriteLine("Cleanup code");
            //}

            //// ----------------------------------------------------------------



        }


    }
}
