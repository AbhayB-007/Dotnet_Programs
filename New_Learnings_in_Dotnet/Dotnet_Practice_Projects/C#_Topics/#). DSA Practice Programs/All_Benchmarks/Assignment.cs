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
    public class Assignment
    {
        private string[] nameList = { "Chris", "Bob", "Mark", "Sam", "Cindy" };

        [Benchmark]
        public void PropertyAssignment()
        {
            foreach (var name in nameList)
            {
                var person = new PersonStruct();
                person.Name = name;
            }
        }

        [Benchmark]
        public void DirectAssignment()
        {
            foreach (var name in nameList)
            {
                var person = new PersonStruct();
                person.Name = name;
            }
        }

    }

    public class PersonClass
    {
        public string Name { get; set; }
        public string name;
    }

    public struct PersonStruct
    {
        public string Name { get; set; }
        public string name;
    }

    public record PersonRecord(string Name);
    public record struct PersonStructRecord(string Name);
}
