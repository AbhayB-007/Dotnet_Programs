using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_C_Sharp_ConsoleApp.csharp_fundamentals_1
{
    public class OperatorOverloading
    {
        static void Main(string[] args)
        {
            Length length1 = new Length(5, 8);
            Length length2 = new Length(3, 5);
            Length totalLength = length1 + length2;
            Console.WriteLine($"Total Length: {totalLength.GetLength()}");

            bool isGreater = length1 > length2;
            Console.WriteLine($"Is the first length greater: {isGreater}");
        }
    }

    public class Length
    {
        int feet, inch;

        public Length(int feet, int inch)
        {
            this.feet = feet;
            this.inch = inch;
        }

        public static Length operator +(Length l1, Length l2)
        {
            int totalInches = (l1.feet * 12 + l1.inch) + (l2.feet * 12 + l2.inch);
            int feet = totalInches / 12;
            int inch = totalInches % 12;
            return new Length(feet, inch);
        }

        public static bool operator >(Length l1, Length l2)
        {
            return (l1.feet * 12 + l1.inch) > (l2.feet * 12 + l2.inch);
        }

        // Matching operator required by the compiler
        public static bool operator <(Length l1, Length l2)
        {
            return (l1.feet * 12 + l1.inch) < (l2.feet * 12 + l2.inch);
        }

        public string GetLength()
        {
            return $"{feet} feet {inch} inch";
        }
    }
}
