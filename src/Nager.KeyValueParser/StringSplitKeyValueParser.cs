namespace Nager.KeyValueParser
{
    /// <summary>
    /// String Split Key Value Parser
    /// </summary>
    public class StringSplitKeyValueParser : IKeyValueParser
    {
        private readonly char _delimiter;
        private readonly char _keyValueSeparator;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringSplitKeyValueParser"/> class.
        /// </summary>
        /// <param name="delimiter">The character that separates key-value pairs (default: ';').</param>
        /// <param name="keyValueSeparator">The character that separates keys from values (default: '=').</param>
        public StringSplitKeyValueParser(
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

            var keyValues = new List<IndexedKeyValueItem>();
            var unrecognizedParts = new List<string>();
            var index = 0;

            var parts = input.Split(_delimiter, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                var cleanPart = part.AsSpan().TrimStart(' ');
                var keyValueSeparatorIndex = cleanPart.IndexOf(_keyValueSeparator);

                if (keyValueSeparatorIndex <= 0)
                {
                    unrecognizedParts.Add(cleanPart.ToString());
                    index++;
                    continue;
                }

                var key = cleanPart[..keyValueSeparatorIndex];
                var value = cleanPart[(keyValueSeparatorIndex + 1)..];

                keyValues.Add(new IndexedKeyValueItem
                {
                    Index = index,
                    Key = key.ToString(),
                    Value = value.ToString()
                });

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
