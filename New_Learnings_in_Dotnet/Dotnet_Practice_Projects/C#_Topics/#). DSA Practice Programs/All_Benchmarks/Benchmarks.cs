using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace Practice.All_Benchmarks
{    
    [MemoryDiagnoser]
    public class Benchmarks
    {
        private readonly List<int> _rawNumbers = Enumerable.Range(1, 5).ToList();

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

        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Benchmarks).Assembly).Run(args, new DebugInProcessConfig());                       
        }
    }   
}
