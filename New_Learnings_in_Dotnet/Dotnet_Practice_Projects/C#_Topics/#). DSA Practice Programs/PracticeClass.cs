using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Practice.All_Benchmarks;
using BenchmarkDotNet.Attributes;

namespace Practice
{
    public class PracticeClass
    {
        public static void TestEncoding()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string amount1 = string.Format("{0:C2}", 1000);
            string amount2 = string.Format(new System.Globalization.CultureInfo("en-IN"), "{0:C2}", 1000);
            var euroCulture = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.InvariantCulture.Clone();
            euroCulture.NumberFormat.CurrencySymbol = "€";
            var amount3 = string.Format(euroCulture, "{0:C2}", 1000);

            Console.WriteLine("amount1 : " + amount1);
            Console.WriteLine("amount2 : " + amount2);
            Console.WriteLine("amount3 : " + amount3);
        }       

        public static void Main(string[] args)
        {
            // TestEncoding();

            // to tun benchmarks in debug mode
            //BenchmarkSwitcher.FromAssembly(typeof(Benchmarks).Assembly).Run(args, new DebugInProcessConfig());

            // to tun benchmarks in release mode
            BenchmarkRunner.Run(typeof(Benchmarks).Assembly);

           





        }
    }
}
