using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_C_Sharp_ConsoleApp.csharp_fundamentals_1
{
    internal class ExtensionMethods
    {
        static void Main(string[] args)
        {
            int number = 12345;
            int digitCount = number.CountDigits();
            Console.WriteLine($"The number {number} has {digitCount} digits.");
        }
    }

    static class Digits
    {
        static public int CountDigits(this int number)
        {
            return number.ToString().Length;
        }
    }
}
