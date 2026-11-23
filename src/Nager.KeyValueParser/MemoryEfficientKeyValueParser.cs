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
                    // A part without a separator does not end the parsing, the parts after it are still parsed
                    unrecognizedParts.Add(value.ToString());
                }
                else
                {
                    keyValues.Add(new IndexedKeyValueItem
                    {
                        Index = index,
                        Key = value[..keyValueSeparatorIndex].ToString(),
                        Value = value[(keyValueSeparatorIndex + 1)..].ToString()
                    });
                }

                if (nextIndexOfDelimiter == -1)
                {
                    break;
                }

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
