# Nager.KeyValueParser

Nager.KeyValueParser is a lightweight and efficient library for parsing key-value pairs from strings. Its flexible design supports various delimiters and provides an easy-to-use API.

## Features
- Support for custom delimiters (e.g., ;, =, etc.)
- High performance with low memory usage
- Easy integration into .NET projects

## Installation
The package is available on [nuget](https://www.nuget.org/packages/Nager.KeyValueParser)
```
PM> install-package Nager.KeyValueParser
```

or

```
dotnet add package Nager.KeyValueParser
```


## Usage
```cs
using Nager.KeyValueParser;

// Example: Parsing key-value pairs using `;` as the pair delimiter and `=` as the key-value separator
var parser = new MemoryEfficientKeyValueParser(';', '=');
var input = "key1=value1;key2=value2";

if (parser.TryParse(input, out var result))
{
    foreach (var kvp in result)
    {
        Console.WriteLine($"{kvp.Key}: {kvp.Value}");
    }
}
else
{
    Console.WriteLine("Failed to parse the input string.");
}
```

## Benchmark

| Method                            | Job        | Toolchain                | IterationCount | LaunchCount | WarmupCount | Mean     | Error   | StdDev  | Gen0   | Gen1   | Allocated |
|---------------------------------- |----------- |------------------------- |--------------- |------------ |------------ |---------:|--------:|--------:|-------:|-------:|----------:|
| TestMemoryEfficientKeyValueParser | DefaultJob | Default                  | Default        | Default     | Default     | 179.6 ns | 1.72 ns | 1.61 ns | 0.0801 | 0.0002 |    1008 B |
| TestStringSplitKeyValueParser     | DefaultJob | Default                  | Default        | Default     | Default     | 232.7 ns | 3.29 ns | 3.08 ns | 0.1116 | 0.0005 |    1400 B |
| TestMemoryEfficientKeyValueParser | MediumRun  | InProcessNoEmitToolchain | 15             | 2           | 10          | 190.2 ns | 2.54 ns | 3.73 ns | 0.0801 | 0.0002 |    1008 B |
| TestStringSplitKeyValueParser     | MediumRun  | InProcessNoEmitToolchain | 15             | 2           | 10          | 240.0 ns | 1.52 ns | 2.23 ns | 0.1116 | 0.0005 |    1400 B |

## License
This project is licensed under the MIT License. See the LICENSE file for details.
