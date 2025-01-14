namespace Nager.KeyValueParser
{
    /// <summary>
    /// Memory Efficient Key Value Parser
    /// </summary>
    public class MemoryEfficientKeyValueParser : IKeyValueParser
    {
        private readonly char _delimiter;
        private readonly char _keyValueSeparator;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryEfficientKeyValueParser"/> class.
        /// </summary>
        /// <param name="delimiter">The character that separates key-value pairs (default: ';').</param>
        /// <param name="keyValueSeparator">The character that separates keys from values (default: '=').</param>
        public MemoryEfficientKeyValueParser(
            char delimiter = ';',
            char keyValueSeparator = '=')
        {
            _delimiter = delimiter;
            _keyValueSeparator = keyValueSeparator;
        }

        /// <inheritdoc />
        public bool TryParse(string input, out ParseResult? parseResult)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                parseResult = null;
                return false;
            }

            var inputSpan = input.AsSpan();
            var nextIndexOfDelimiter = 0;

            var keyValues = new List<IndexedKeyValueItem>();
            var unrecognizedParts = new List<string>();
            var index = 0;

            while (nextIndexOfDelimiter != -1)
            {
                nextIndexOfDelimiter = inputSpan.IndexOf(_delimiter);

                ReadOnlySpan<char> value;
                if (nextIndexOfDelimiter == -1)
                {
                    value = inputSpan.Trim();
                }
                else
                {
                    value = inputSpan[..nextIndexOfDelimiter].Trim();
                }

                var keyValueSeparatorIndex = value.IndexOf(_keyValueSeparator);
                if (keyValueSeparatorIndex == -1)
                {
                    unrecognizedParts.Add(value.ToString());
                    break;
                }

                var key = value[..keyValueSeparatorIndex];
                var dataStartIndex = keyValueSeparatorIndex + 1;

                if (dataStartIndex > value.Length)
                {
                    //failure...
                    break;
                }

                keyValues.Add(new IndexedKeyValueItem
                {
                    Index = index,
                    Key = key.ToString(),
                    Value = value[dataStartIndex..].ToString()
                });

                inputSpan = inputSpan[(nextIndexOfDelimiter + 1)..];
                if (inputSpan.IsEmpty)
                {
                    break;
                }

                index++;
            }

            parseResult = new ParseResult
            {
                KeyValues = [.. keyValues],
                UnrecognizedParts = unrecognizedParts.Count > 0 ? [.. unrecognizedParts] : null
            };

            return true;
        }
    }
}
