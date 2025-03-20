using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Mathematics;
using System.Text;

namespace Practice.All_Benchmarks
{
    [MemoryDiagnoser]
    [ShortRunJob]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn(NumeralSystem.Arabic)]
    public class Benchmarks
    {
        private readonly List<int> _rawNumbers = Enumerable.Range(1, 5).ToList();
        private string value = "test";
        private string testString = "TEST";
        private string[] nameList = { "Chris", "Bob", "Mark", "Sam", "Cindy" };

        // Find x in list --------------------------------------
        //[Benchmark]
        public int FindRaw()
        {
            return _rawNumbers.Find(x => x == 3);
        }

        //[Benchmark]
        public int FirstOrDefaultRaw()
        {
            return _rawNumbers.FirstOrDefault(x => x == 3);
        }

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
            var x = Strings.NaiveSplitName("Bindal, Abhay");
        }

        //[Benchmark]
        public void SplitSplit()
        {
            var x = Strings.SplitSplitName("Bindal, Abhay");
        }

        //[Benchmark]
        public void SpanSplit()
        {
            var x = Strings.SpanSplitName("Bindal, Abhay");
        }

        // Compare Strings -------------------------------------
        //[Benchmark]
        public void EqualityComparision()
        {
            var compare = testString == "TEST";
        }

        //[Benchmark]
        public void EqualComparision()
        {
            var compare = testString.Equals("TEST");
        }

        //[Benchmark]
        public void StringCompareComparision()
        {
            var compare = string.Compare(testString, "TEST");
        }

        // Loops -----------------------------------------------
        [Benchmark]
        public void ForEachLoop()
        {
            foreach (var item in nameList)
            {
                var x = item;
            }
        }

        [Benchmark]
        public void ForLoop()
        {
            var len = nameList.Length;
            for (var i = 0; i < len; i++)
            {
                var x = nameList[i];
            }
        }

        static void Main1(string[] args)
        {
            //// to tun benchmarks in debug mode
            //BenchmarkSwitcher.FromAssembly(typeof(Benchmarks).Assembly).Run(args, new DebugInProcessConfig());
            //// to tun benchmarks in release mode
            //BenchmarkRunner.Run(typeof(Benchmarks).Assembly);
        }
    }

    public class Persom
    {

    }

    public class Strings
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
