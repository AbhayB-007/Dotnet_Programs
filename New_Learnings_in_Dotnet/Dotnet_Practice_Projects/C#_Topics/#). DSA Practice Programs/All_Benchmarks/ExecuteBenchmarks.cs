using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Practice.All_Benchmarks;

// To run benchmarks in debug mode
// BenchmarkSwitcher.FromAssembly(typeof(ExtensionMethods).Assembly).Run(args, new DebugInProcessConfig());

// To run all the benchmarks in assembly in release mode
// BenchmarkRunner.Run(typeof(ExtensionMethods).Assembly);

// To run benchmarks in release mode
BenchmarkRunner.Run(typeof(ExtensionMethods));
