using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.Logging;
using Practice.All_Benchmarks;

namespace Practice
{
    public class PracticeClass
    {
        public static void TestEncoding()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string amount1 = string.Format("{0:C2}", 1000);
            string amount2 = string.Format(
                new System.Globalization.CultureInfo("en-IN"),
                "{0:C2}",
                1000
            );
            var euroCulture = (System.Globalization.CultureInfo)
                System.Globalization.CultureInfo.InvariantCulture.Clone();
            euroCulture.NumberFormat.CurrencySymbol = "€";
            var amount3 = string.Format(euroCulture, "{0:C2}", 1000);

            Console.WriteLine("amount1 : " + amount1);
            Console.WriteLine("amount2 : " + amount2);
            Console.WriteLine("amount3 : " + amount3);
        }

        public static void Main()
        {
            TestEncoding();
        }
    }
}
