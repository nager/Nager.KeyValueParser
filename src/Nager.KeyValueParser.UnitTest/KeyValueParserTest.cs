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
    }
}
