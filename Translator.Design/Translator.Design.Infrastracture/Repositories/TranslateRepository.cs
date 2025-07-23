using Microsoft.Extensions.Configuration;
using OllamaSharp;
using OllamaSharp.Models;
using Translator.Design.Application.IRepositories;
using Translator.Design.Domain.Responses;

namespace Translator.Design.Infrastracture.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TranslateRepository : ITranslateRepository
    {
        #region Declarations

        private readonly OllamaApiClient _client;
        private readonly IConfiguration _configuration;

        #endregion Declarations

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public TranslateRepository(IConfiguration configuration)
        {
            //instantiate configuration
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            //set the ollama configuration
            string baseUrl = configuration["Ollama:BaseUrl"] ?? string.Empty;
            string selectedModel = configuration["Ollama:SelectedModel"] ?? string.Empty;
            int timeout = Convert.ToInt32(configuration["Ollama:Timeout"] ?? "1");

            //create an http client
            HttpClient httpClient = new()
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = TimeSpan.FromMinutes(timeout)
            };

            //create the ollama api client
            _client = new OllamaApiClient(httpClient)
            { SelectedModel = selectedModel };
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<TranslateRes> Translate(string input)
        {
            #region variables

            string tokenRes = string.Empty;
            string prompt = (_configuration["Ollama:Prompt"] ?? string.Empty)
                .Replace("{input}", input, StringComparison.OrdinalIgnoreCase);

            #endregion variables

            await foreach (GenerateResponseStream? token in _client.GenerateAsync(prompt))
            {
                if (token?.Response != null)
                    tokenRes += token.Response;
            }

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