using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_C_Sharp_ConsoleApp.csharp_fundamentals_1
{
    internal class Reflection
    {
        static void Main(string[] args)
        {
            // string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            // OR
            string path = @"C:/Users/Abhay/OneDrive/Desktop/Abhay_Programs_and_Notes/My_Git_Repositories/Dotnet_Programs/New_Learnings_in_Dotnet/1_C_Sharp_Practice_Projects/C#_Topics/Practice_C_Sharp_ConsoleApp/bin/Debug/net8.0/Practice_C_Sharp_ConsoleApp.dll";

            // Get the current assembly
            //var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            // OR
            var assembly = System.Reflection.Assembly.LoadFrom(path);

            // Get all types in the assembly
            var classes = assembly.GetTypes();
            
            // Print the names of all types
            foreach (var type in classes)
            {
                Console.WriteLine("Class: " + type.FullName);

                // Get all methods in the type
                var methods = type.GetMethods();
                foreach (var method in methods)
                {
                    Console.WriteLine("  Method: " + method.Name);

                    // Get all parameters of the method
                    var parameters = method.GetParameters();
                    foreach (var parameter in parameters)
                    {
                        Console.WriteLine("    Parameter: " + parameter.Name + " of type " + parameter.ParameterType);
                    }
                }
            }
        }
    }
}

