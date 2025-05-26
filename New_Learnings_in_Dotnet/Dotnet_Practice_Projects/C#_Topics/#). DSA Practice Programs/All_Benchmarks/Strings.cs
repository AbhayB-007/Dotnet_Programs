using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.All_Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]    
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn(NumeralSystem.Arabic)]
    public class Strings
    {
        private string value = "test";
        private string testString = "TEST";

        // Strings ---------------------------------------------
        //[Benchmark]
        public string BuildStringBadly()
        {
            for (var i = 0; i < 50; i++)
            {
                value += " " + "test";
            }
            return value;
        }

        //[Benchmark]
        public string BuildStringBetter()
        {
            var sb = new StringBuilder(value);
            for (var i = 0; i < 50; i++)
            {
                // will execute faster instead of using " ", as internally it typecast it to char
                sb.Append(' ');
                sb.Append("test");
            }
            return sb.ToString();
        }

        // String Split ----------------------------------------
        //[Benchmark]
        public void NaiveSplit()
        {
            var x = StringMethods.NaiveSplitName("Bindal, Abhay");
        }

        //[Benchmark]
        public void SplitSplit()
        {
            var x = StringMethods.SplitSplitName("Bindal, Abhay");
        }

        //[Benchmark]
        public void SpanSplit()
        {
            var x = StringMethods.SpanSplitName("Bindal, Abhay");
        }

        // Compare Strings -------------------------------------
        [Benchmark]
        public void EqualityComparision()
        {
            var compare = testString == "TEST";
        }

        [Benchmark]
        public void EqualComparision()
        {
            var compare = testString.Equals("TEST");
        }

        [Benchmark]
        public void StringCompareComparision()
        {
            var compare = string.Compare(testString, "TEST");
        }
    }

    public class StringMethods
    {
        public static (string lastName, string firstName) NaiveSplitName(string name)
        {
            var commaPos = name.IndexOf(',');
            var lastName = name.Substring(0, commaPos);
            var firstName = name.Substring(commaPos + 2);
            return (lastName, firstName);
        }

        public static (string lastName, string firstName) SplitSplitName(string name)
        {
            var nameArray = name.Split(',');
            return (nameArray[0].Trim(), nameArray[1].Trim());
        }

        public static (string lastName, string firstName) SpanSplitName(string name)
        {
            ReadOnlySpan<char> nameSpan = name.AsSpan();
            var lastName = nameSpan.Slice(0, name.IndexOf(",")).ToString();
            var firstName = nameSpan.Slice(name.IndexOf(",") + 2).ToString();
            return (lastName, firstName);
        }
    }
}
