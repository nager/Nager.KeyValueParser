namespace Nager.KeyValueParser
{
    /// <summary>
    /// Interface Key Value Parser
    /// </summary>
    public interface IKeyValueParser
    {
        /// <summary>
        /// Try Parse the input
        /// </summary>
        /// <param name="input"></param>
        /// <param name="parseResult"></param>
        /// <returns></returns>
        bool TryParse(string input, out ParseResult? parseResult);
    }
}
