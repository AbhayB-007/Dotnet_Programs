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
    public class DataTypes
    {
        [Benchmark]
        public void TypeInt()
        {
            int x = 200;
            for (var i = 0; i < x; i++)
            {
                var y = i;
                y++;
            }
        }

        [Benchmark]
        public void TypeByte()
        {
            byte x = 200;
            for (var i = 0; i < x; i++)
            {
                var y = i;
                y++;
            }
        }

        [Benchmark]
        public void TypeLong()
        {
            long x = 200;
            for (var i = 0; i < x; i++)
            {
                var y = i;
                y++;
            }
        }

        [Benchmark]
        public void TypeFloat()
        {
            float x = 200;
            for (var i = 0; i < x; i++)
            {
                var y = i;
                y++;
            }
        }

        [Benchmark]
        public void TypeDouble()
        {
            double x = 200;
            for (var i = 0; i < x; i++)
            {
                var y = i;
                y++;
            }
        }

        [Benchmark]
        public void TypeDecimal()
        {
            decimal x = 200;
            for (var i = 0; i < x; i++)
            {
                var y = i;
                y++;
            }
        }
    }
}
