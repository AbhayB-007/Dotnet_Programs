using BenchmarkDotNet.Attributes;
using System.Runtime.InteropServices;

namespace Linq_In_CSharp
{
    public class PracticeLinq
    {
        static void Main(string[] args)
        {
            
        }
        public int[] ArrayData { get; set; } = Array.Empty<int>();
        public List<int> ListData { get; set; } = new();

        public IEnumerable<int> LazyEvaluatedData
        {
            get { foreach (int x in ArrayData) yield return x; }
        }

        private static IEnumerable<int> GetRandomizeedNumbers()
        {
            var rng = new Random(42);
            while (true) yield return rng.Next(1000);
        }
        [Params(100, 1_000, 10_000, 30_000)] public int N { get; set; } = 0;

        [GlobalSetup]
        public void Setup()
        {
            ArrayData = GetRandomizeedNumbers().Take(N).ToArray();
            ListData = ArrayData.ToList();
        }

        private int LocalArrayForSum(int[] data)
        {
            int sum = 0;
            for (int i = 0; i < data.Length; i++) sum += data[i];
            return sum;
        }

        private int MemberArrayForSum(PracticeLinq obj)
        {
            int sum = 0;
            for (int i = 0; i < obj.ArrayData.Length; i++) sum += obj.ArrayData[i];
            return sum;
        }

        private int ArrayForeachSum(int[] data)
        {
            int sum = 0;
            foreach (int x in data) sum += x;
            return sum;
        }

        private int ListForSum(List<int> data)
        {
            int sum = 0;
            for (int i = 0; i < data.Count; i++) sum += data[i];
            return sum;
        }

        private int ListForeachSum(List<int> data)
        {
            int sum = 0;
            foreach (int x in data) sum += x;
            return sum;
        }

        private int EnumerableForeachSum(IEnumerable<int> data)
        {
            int sum = 0;
            foreach (var x in data) sum += x;
            return sum;
        }

        private int AggregateSum(IEnumerable<int> data) =>
            data.Aggregate(0, (sum, x) => sum + x);

        private int LinqSum(IEnumerable<int> data) => data.Sum();

        private int SpanForSum(Span<int> span)
        {
            int sum = 0;
            for (int i = 0; i < span.Length; i++) sum += span[i];
            return sum;
        }

        private int SpanForeachSum(Span<int> span)
        {
            int sum = 0;
            foreach (int x in span) sum += x;
            return sum;
        }

        private int ReadOnlySpanForSum(ReadOnlySpan<int> span)
        {
            int sum = 0;
            for (int i = 0; i < span.Length; i++) sum += span[i];
            return sum;
        }

        private int ReadOnlySpanForeachSum(ReadOnlySpan<int> span)
        {
            int sum = 0;
            foreach (int x in span) sum += x;
            return sum;
        }

        [Benchmark(Baseline = true)] public int LocalArrayFor() => LocalArrayForSum(ArrayData);
        [Benchmark] public int MemberArrayFor() => MemberArrayForSum(this);
        [Benchmark] public int ArrayForeach() => ArrayForeachSum(ArrayData);
        [Benchmark] public int ArrayEnumerableForeach() => EnumerableForeachSum(ArrayData);
        [Benchmark] public int ArrayAggregate() => AggregateSum(ArrayData);
        [Benchmark] public int ArrayLinq() => LinqSum(ArrayData);
        [Benchmark] public int ArraySpanFor() => SpanForSum(ArrayData);
        [Benchmark] public int ArraySpanForeach() => SpanForeachSum(ArrayData);
        [Benchmark] public int ArrayReadOnlySpanFor() => ReadOnlySpanForSum(ArrayData);
        [Benchmark] public int ArrayReadOnlySpanForeach() => ReadOnlySpanForeachSum(ArrayData);


        [Benchmark] public int ListFor() => ListForSum(ListData);
        [Benchmark] public int ListForeach() => ListForeachSum(ListData);
        [Benchmark] public int ListEnumerableForeach() => EnumerableForeachSum(ListData);
        [Benchmark] public int ListAggregate() => AggregateSum(ListData); [Benchmark] public int ListLinq() => LinqSum(ListData);
        [Benchmark] public int ListSpanFor() => SpanForSum(CollectionsMarshal.AsSpan(ListData));
        [Benchmark] public int ListSpanForeach() => SpanForeachSum(CollectionsMarshal.AsSpan(ListData));
        [Benchmark] public int ListReadOnlySpanFor() => ReadOnlySpanForSum(CollectionsMarshal.AsSpan(ListData));
        [Benchmark] public int ListReadOnlySpanForeach() => ReadOnlySpanForeachSum(CollectionsMarshal.AsSpan(ListData));
        [Benchmark] public int LazyEvaluatedForeach() => EnumerableForeachSum(LazyEvaluatedData);
        [Benchmark] public int LazyEvaluatedAggregate() => AggregateSum(LazyEvaluatedData);
        [Benchmark] public int LazyEvaluatedLinq() => LinqSum(LazyEvaluatedData);
        [Benchmark] public int LazyEvaluatedToArrayForeach() => ArrayForeachSum(LazyEvaluatedData.ToArray());
        [Benchmark] public int LazyEvaluatedToListForeach() => ListForeachSum(LazyEvaluatedData.ToList());
    }
}