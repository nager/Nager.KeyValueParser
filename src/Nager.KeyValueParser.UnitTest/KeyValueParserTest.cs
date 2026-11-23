namespace Nager.KeyValueParser.UnitTest
{
    [TestClass]
    public sealed class KeyValueParserTest
    {
        private static IEnumerable<IKeyValueParser> GetParsers(char delimiter)
        {
            yield return new StringSplitKeyValueParser(delimiter, keyValueSeparator: '=');
            yield return new MemoryEfficientKeyValueParser(delimiter, keyValueSeparator: '=');
        }

        [TestMethod]
        public void Parse_ValidDmarcData_ReturnsParseResult()
        {
            foreach (var parser in GetParsers(';'))
            {
                var isParsable = parser.TryParse("v=DMARC1; p=reject;", out var parseResult);

                Assert.IsTrue(isParsable);
                Assert.IsNotNull(parseResult);

                var firstKeyValue = parseResult.KeyValues.Single(o => o.Index == 0);
                Assert.AreEqual("v", firstKeyValue.Key);
                Assert.AreEqual("DMARC1", firstKeyValue.Value);

                var secondKeyValue = parseResult.KeyValues.Single(o => o.Index == 1);
                Assert.AreEqual("p", secondKeyValue.Key);
                Assert.AreEqual("reject", secondKeyValue.Value);
            }
        }

        [TestMethod]
        public void Parse_ValidDmarcDataWithoutSemeliconAtTheEnd_ReturnsParseResult()
        {
            foreach (var parser in GetParsers(';'))
            {
                var isParsable = parser.TryParse("v=DMARC1; p=reject", out var parseResult);

                Assert.IsTrue(isParsable);
                Assert.IsNotNull(parseResult);

                var firstKeyValue = parseResult.KeyValues.Single(o => o.Index == 0);
                Assert.AreEqual("v", firstKeyValue.Key);
                Assert.AreEqual("DMARC1", firstKeyValue.Value);

                var secondKeyValue = parseResult.KeyValues.Single(o => o.Index == 1);
                Assert.AreEqual("p", secondKeyValue.Key);
                Assert.AreEqual("reject", secondKeyValue.Value);
            }
        }

        [TestMethod]
        public void Parse_ValidDmarcDataWithDoubleSpace_ReturnsParseResult()
        {
            foreach (var parser in GetParsers(';'))
            {
                var isParsable = parser.TryParse("v=DMARC1;  p=reject", out var parseResult);

                Assert.IsTrue(isParsable);
                Assert.IsNotNull(parseResult);

                var firstKeyValue = parseResult.KeyValues.Single(o => o.Index == 0);
                Assert.AreEqual("v", firstKeyValue.Key);
                Assert.AreEqual("DMARC1", firstKeyValue.Value);

                var secondKeyValue = parseResult.KeyValues.Single(o => o.Index == 1);
                Assert.AreEqual("p", secondKeyValue.Key);
                Assert.AreEqual("reject", secondKeyValue.Value);
            }
        }

        [TestMethod]
        public void Parse_ValidDkimData_ReturnsParseResult()
        {
            foreach (var parser in GetParsers(';'))
            {
                var isParsable = parser.TryParse("v=DKIM1; k=rsa; p=MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC3QEKyU1fSma0axspqYK5iAj+54lsAg4qRRCnpKK68hawSd8zpsDz77ntGCR0X2mHVvkf0WEOIqaspaG/A5IGxieiWer+wBX8lW2tE4NHTE0PLhHqL0uD2sif2pKoPR3Wr6n/rbiihGYCIzvuY4/U5GigNUGls/QUbCPRyzho30wIDAQAB", out var parseResult);

                Assert.IsTrue(isParsable);
                Assert.IsNotNull(parseResult);

                var firstKeyValue = parseResult.KeyValues.Single(o => o.Index == 0);
                Assert.AreEqual("v", firstKeyValue.Key);
                Assert.AreEqual("DKIM1", firstKeyValue.Value);

                var secondKeyValue = parseResult.KeyValues.Single(o => o.Index == 1);
                Assert.AreEqual("k", secondKeyValue.Key);
                Assert.AreEqual("rsa", secondKeyValue.Value);

                var thirdKeyValue = parseResult.KeyValues.Single(o => o.Index == 2);
                Assert.AreEqual("p", thirdKeyValue.Key);
                Assert.AreEqual("MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC3QEKyU1fSma0axspqYK5iAj+54lsAg4qRRCnpKK68hawSd8zpsDz77ntGCR0X2mHVvkf0WEOIqaspaG/A5IGxieiWer+wBX8lW2tE4NHTE0PLhHqL0uD2sif2pKoPR3Wr6n/rbiihGYCIzvuY4/U5GigNUGls/QUbCPRyzho30wIDAQAB", thirdKeyValue.Value);
            }
        }

        [TestMethod]
        public void Parse_PartWithoutKeyValueSeparator_ParsesFollowingParts()
        {
            foreach (var parser in GetParsers(';'))
            {
                var isParsable = parser.TryParse("v=DMARC1; p=reject; invalid; rua=mailto:dmarc@example.com", out var parseResult);

                Assert.IsTrue(isParsable);
                Assert.IsNotNull(parseResult);
                Assert.AreEqual(3, parseResult.KeyValues.Length);

                var secondKeyValue = parseResult.KeyValues.Single(o => o.Index == 1);
                Assert.AreEqual("p", secondKeyValue.Key);
                Assert.AreEqual("reject", secondKeyValue.Value);

                // The part without a separator has the index 2
                var lastKeyValue = parseResult.KeyValues.Single(o => o.Index == 3);
                Assert.AreEqual("rua", lastKeyValue.Key);
                Assert.AreEqual("mailto:dmarc@example.com", lastKeyValue.Value);

                Assert.IsNotNull(parseResult.UnrecognizedParts);
                CollectionAssert.AreEqual(new[] { "invalid" }, parseResult.UnrecognizedParts);
            }
        }

        [TestMethod]
        public void Parse_MultiplePartsWithoutKeyValueSeparator_ReturnsAllUnrecognizedParts()
        {
            foreach (var parser in GetParsers(';'))
            {
                var isParsable = parser.TryParse("a=1;b;c=3;d;e=5", out var parseResult);

                Assert.IsTrue(isParsable);
                Assert.IsNotNull(parseResult);

                CollectionAssert.AreEqual(new[] { "a", "c", "e" }, parseResult.KeyValues.Select(o => o.Key).ToArray());
                CollectionAssert.AreEqual(new[] { "1", "3", "5" }, parseResult.KeyValues.Select(o => o.Value).ToArray());
                CollectionAssert.AreEqual(new[] { 0, 2, 4 }, parseResult.KeyValues.Select(o => o.Index).ToArray());

                Assert.IsNotNull(parseResult.UnrecognizedParts);
                CollectionAssert.AreEqual(new[] { "b", "d" }, parseResult.UnrecognizedParts);
            }
        }

        [TestMethod]
        public void Parse_PartWithoutKeyValueSeparatorAtTheEnd_ReturnsUnrecognizedPart()
        {
            foreach (var parser in GetParsers(';'))
            {
                var isParsable = parser.TryParse("v=DMARC1; p=reject; invalid", out var parseResult);

                Assert.IsTrue(isParsable);
                Assert.IsNotNull(parseResult);
                Assert.AreEqual(2, parseResult.KeyValues.Length);

                Assert.IsNotNull(parseResult.UnrecognizedParts);
                CollectionAssert.AreEqual(new[] { "invalid" }, parseResult.UnrecognizedParts);
            }
        }

        [TestMethod]
        public void Parse_OnlyPartWithoutKeyValueSeparator_ReturnsUnrecognizedPart()
        {
            foreach (var parser in GetParsers(';'))
            {
                var isParsable = parser.TryParse("invalid", out var parseResult);

                Assert.IsTrue(isParsable);
                Assert.IsNotNull(parseResult);
                Assert.AreEqual(0, parseResult.KeyValues.Length);

                Assert.IsNotNull(parseResult.UnrecognizedParts);
                CollectionAssert.AreEqual(new[] { "invalid" }, parseResult.UnrecognizedParts);
            }
        }

        [TestMethod]
        public void Parse_ValueWithKeyValueSeparator_ReturnsCompleteValue()
        {
            foreach (var parser in GetParsers(';'))
            {
                var isParsable = parser.TryParse("v=DKIM1; p=dGVzdA==; n=a=b", out var parseResult);

                Assert.IsTrue(isParsable);
                Assert.IsNotNull(parseResult);
                Assert.IsNull(parseResult.UnrecognizedParts);

                var secondKeyValue = parseResult.KeyValues.Single(o => o.Index == 1);
                Assert.AreEqual("p", secondKeyValue.Key);
                Assert.AreEqual("dGVzdA==", secondKeyValue.Value);

                var thirdKeyValue = parseResult.KeyValues.Single(o => o.Index == 2);
                Assert.AreEqual("n", thirdKeyValue.Key);
                Assert.AreEqual("a=b", thirdKeyValue.Value);
            }
        }
    }
}
