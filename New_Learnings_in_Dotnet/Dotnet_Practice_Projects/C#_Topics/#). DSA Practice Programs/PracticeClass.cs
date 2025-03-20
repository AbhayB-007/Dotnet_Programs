using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice
{
    public class PracticeClass
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            string amount1 = string.Format("{0:C2}", 1000);

            string amount2 = string.Format(new System.Globalization.CultureInfo("fr-FR"), "{0:C2}", 1000);

            var euroCulture = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.InvariantCulture.Clone();
            euroCulture.NumberFormat.CurrencySymbol = "€";

            var amount3 = string.Format(euroCulture, "{0:C2}", 1000);

            Console.WriteLine("amount1 : " + amount1);
            Console.WriteLine("amount2 : " + amount2);
            Console.WriteLine("amount3 : " + amount3);
        }
    }
}
