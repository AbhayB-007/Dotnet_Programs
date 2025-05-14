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
    public class ExtensionMethods
    {
        private readonly List<int> _rawNumbers = Enumerable.Range(1, 5).ToList();
        private string[] nameList = { "Chris", "Bob", "Mark", "Sam", "Cindy" };

        [Benchmark]
        public int FindRaw()
        {
            return _rawNumbers.Find(x => x == 3);
        }

        [Benchmark]
        public int FirstOrDefaultRaw()
        {
            return _rawNumbers.FirstOrDefault(x => x == 3);
        }
    }
}

  

