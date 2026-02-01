using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_C_Sharp_ConsoleApp.csharp_fundamentals_1
{
    public class DataTypes
    {
        public static void AllDatatypes()
        {
            Console.WriteLine("Data Types in C#");
            /*
             * byte a;
             * short b;
             * int c;  
             * long d;
             * float e;
             * double f;
             * decimal g;
             * char h;
             * bool i;
            */

            // value types
            sbyte sbyteMax = sbyte.MaxValue;
            sbyte sbyteMin = sbyte.MinValue;
            Console.WriteLine($"sbyte Max: {sbyteMax}, sbyte Min: {sbyteMin}");

            ushort ushortMax = ushort.MaxValue;
            ushort ushortMin = ushort.MinValue;
            Console.WriteLine($"ushort Max: {ushortMax}, ushort Min: {ushortMin}");

            byte byteMax = byte.MaxValue;
            byte byteMin = byte.MinValue;
            Console.WriteLine($"byte Max: {byteMax}, byte Min: {byteMin}");

            short shortMax = short.MaxValue;
            short shortMin = short.MinValue;
            Console.WriteLine($"short Max: {shortMax}, short Min: {shortMin}");

            int intMax = int.MaxValue;
            int intMin = int.MinValue;
            Console.WriteLine($"int Max: {intMax}, int Min: {intMin}");

            long longMax = long.MaxValue;
            long longMin = long.MinValue;
            Console.WriteLine($"long Max: {longMax}, long Min: {longMin}");

            float floatMax = float.MaxValue;
            float floatMin = float.MinValue;
            Console.WriteLine($"float Max: {floatMax}, float Min: {floatMin}");

            double doubleMax = double.MaxValue;
            double doubleMin = double.MinValue;
            Console.WriteLine($"double Max: {doubleMax}, double Min: {doubleMin}");

            decimal decimalMax = decimal.MaxValue;
            decimal decimalMin = decimal.MinValue;
            Console.WriteLine($"decimal Max: {decimalMax}, decimal Min: {decimalMin}");

            char charMax = char.MaxValue;
            char charMin = char.MinValue;
            Console.WriteLine($"char Max: {(int)charMax}, char Min: {(int)charMin}");

            bool boolTrue = true;
            bool boolFalse = false;
            Console.WriteLine($"bool True: {boolTrue}, bool False: {boolFalse}");

            // Reference Types
            object obj = new object();
            Console.WriteLine($"object: {obj.ToString()}");

            string str = "Hello, World!";
            Console.WriteLine($"string: {str}");            

        }
        
    }
}
