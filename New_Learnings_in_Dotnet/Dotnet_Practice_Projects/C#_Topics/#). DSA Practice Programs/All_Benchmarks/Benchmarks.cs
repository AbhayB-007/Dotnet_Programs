using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Mathematics;
using System.Text;

namespace Practice.All_Benchmarks
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn(NumeralSystem.Arabic)]
    public class Benchmarks
    {
        private readonly List<int> _rawNumbers = Enumerable.Range(1, 5).ToList();
        private string value = "test";
        private string testString = "TEST";

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
                sb.Append(" ");
                sb.Append("test");
            }
            return sb.ToString();
        }

        //[Benchmark]
        public void NaiveSplit()
        {
            var x = PracticeClass.NaiveSplitName("Bindal, Abhay");
        }

        //[Benchmark]
        public void SplitSplit()
        {
            var x = PracticeClass.SplitSplitName("Bindal, Abhay");
        }

        //[Benchmark]
        public void SpanSplit()
        {
            var x = PracticeClass.SpanSplitName("Bindal, Abhay");
        }

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

        static void Main1(string[] args)
        {
            //// to tun benchmarks in debug mode
            //BenchmarkSwitcher.FromAssembly(typeof(Benchmarks).Assembly).Run(args, new DebugInProcessConfig());
            //// to tun benchmarks in release mode
            //BenchmarkRunner.Run(typeof(Benchmarks).Assembly);
        }
    }
}
