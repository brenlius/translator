using Microsoft.Extensions.Configuration;
using Translator.Design.Application.IRepositories;
using Translator.Design.Connector.Clients;
using Translator.Design.Domain.Responses;

namespace Translator.Design.Infrastracture.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ollamaClient"></param>
    /// <param name="openRouterClient"></param>
    /// <param name="config"></param>
    public sealed class TranslateRepository(OllamaClient ollamaClient, OpenRouterClient openRouterClient, IConfiguration config) : ITranslateRepository
    {
        #region Declarations

        private readonly OllamaClient _ollamaClient = ollamaClient;
        private readonly OpenRouterClient _openRouterClient = openRouterClient;
        private readonly string _provider = config["TranslationProvider"] ?? string.Empty;

        #endregion Declarations

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<TranslateRes> Translate(string input)
        {
            #region Variables

            string? tokenRes;

            #endregion Variables

            switch (_provider.ToLowerInvariant())
            {
                case "ollama":
                    tokenRes = await _ollamaClient.Translate(input);
                    break;
                case "openrouter":
                    tokenRes = await _openRouterClient.Translate(input);
                    break;
                default:
                    tokenRes = await _openRouterClient.Translate(input);
                    break;
            }

            if (string.IsNullOrEmpty(tokenRes))
                throw new ArgumentException("Translation not found.", nameof(input));

            return ParseResults(tokenRes);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenRes"></param>
        /// <returns></returns>
        private TranslateRes ParseResults(string tokenRes)
        {
            TranslateRes translateRes = new();

            foreach (string rawLine in tokenRes.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                string line = rawLine.Replace("**", "").Trim();

                if (line.StartsWith("Corrected", StringComparison.OrdinalIgnoreCase))
                    translateRes.CorrectedRomaji = line.Split(':', 2)[1].Trim();
                else if (line.StartsWith("Japanese", StringComparison.OrdinalIgnoreCase))
                    translateRes.Japanese = line.Split(':', 2)[1].Trim();
                else if (line.StartsWith("English", StringComparison.OrdinalIgnoreCase))
                    translateRes.English = line.Split(':', 2)[1].Trim();
            }

            return translateRes;
        }

        #endregion Private Methods

    }
}