using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

// Run Benchmarks
BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());




