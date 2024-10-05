using BenchmarkDotNet.Attributes;

namespace FirstOrDefault_or_Find
{
    [MemoryDiagnoser(false)]
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
    }
}
