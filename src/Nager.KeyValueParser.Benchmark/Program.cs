// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using Nager.KeyValueParser.Benchmark;

var summary = BenchmarkRunner.Run<ParseBenchmark>();