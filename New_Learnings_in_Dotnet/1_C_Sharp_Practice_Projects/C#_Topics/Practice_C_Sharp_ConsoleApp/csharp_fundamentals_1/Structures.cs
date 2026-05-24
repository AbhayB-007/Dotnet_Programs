using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_C_Sharp_ConsoleApp.csharp_fundamentals_1
{
    internal class Structures
    {
        static void Main(string[] args)
        {
            MyStruct myStruct1 = new MyStruct() { Val = 10 };
            MyStruct myStruct2 = myStruct1; // This creates a copy of myStruct1
            Console.WriteLine($"myStruct1.Val: {myStruct1.Val}"); // Output: 10
            Console.WriteLine($"myStruct2.Val: {myStruct2.Val}"); // Output: 10

            // Modifying myStruct2 does not affect myStruct1
            myStruct2.Val = 20; 
            Console.WriteLine($"After modifying myStruct2");
            Console.WriteLine($"myStruct1.Val: {myStruct1.Val}"); // Output: 10
            Console.WriteLine($"myStruct2.Val: {myStruct2.Val}"); // Output: 20
        }
    }

    struct MyStruct 
    {
        public int Val {  get; set; }    
    }



}
