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
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn(NumeralSystem.Arabic)]
    public class DateStuff
    {
        private string dateString = "2025-02-22";

        [Benchmark]
        public void Parse()
        {
            var date = DateTime.Parse(dateString);
            var year = date.Year;
        }

        [Benchmark]
        public void DirectCreate()
        {
            var date = new DateTime(2025, 2, 22);
            var year = date.Year;
        }

        [Benchmark]
        public void CustomParsing()
        {
            ReadOnlySpan<char> dateSpan = dateString.AsSpan();

            var year = int.Parse(dateSpan.Slice(0, 4));
            var month = int.Parse(dateSpan.Slice(5, 2));
            var day = int.Parse(dateSpan.Slice(8, 2));

            var date = new DateTime(year, month, day);
            var year2 = date.Year;
        }
    }
}
