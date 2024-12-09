// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;

namespace Nager.KeyValueParser.Benchmark
{
    [Config(typeof(AntiVirusFriendlyConfig))]
    [MemoryDiagnoser]
    [SimpleJob]
    public class ParseBenchmark
    {
        private IKeyValueParser _memoryEfficientKeyValueParser;
        private IKeyValueParser _stringSplitKeyValueParser;

        private readonly string testInput = "v=DMARC1;p=none;rua=mailto:dmarc@example.com;ruf=mailto:dmarc@example.com;rf=afrf;pct=100";

        [GlobalSetup]
        public void Setup()
        {
            this._memoryEfficientKeyValueParser = new MemoryEfficientKeyValueParser(';', '=');
            this._stringSplitKeyValueParser = new StringSplitKeyValueParser(';', '=');
        }

        [Benchmark]
        public void TestMemoryEfficientKeyValueParser()
        {
            this._memoryEfficientKeyValueParser.TryParse(testInput, out var parseResult);
        }

        [Benchmark]
        public void TestStringSplitKeyValueParser()
        {
            this._stringSplitKeyValueParser.TryParse(testInput, out var parseResult);
        }
    }
}
