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

## License
This project is licensed under the MIT License. See the LICENSE file for details.
