using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Order;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.All_Benchmarks
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn(NumeralSystem.Arabic)]
    public class Loops
    {
        private string[] nameList = { "Chris", "Bob", "Mark", "Sam", "Cindy" };

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
    }
}
