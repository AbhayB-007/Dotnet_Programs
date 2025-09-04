using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Microsoft.VisualBasic;


// To run benchmarks in debug mode
BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());

// To run all the benchmarks in assembly in release mode
// BenchmarkRunner.Run(typeof(Program).Assembly);

// To run benchmarks in release mode
// BenchmarkRunner.Run(typeof(Program));
