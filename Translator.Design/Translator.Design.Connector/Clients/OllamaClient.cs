using Microsoft.Extensions.Options;
using OllamaSharp;
using OllamaSharp.Models;
using Translator.Design.Connector.Configs;

namespace Translator.Design.Connector.Clients
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="options"></param>
    public sealed class OllamaClient(HttpClient httpClient, IOptions<OllamaConfig> options)
    {
        #region Declarations

        private readonly HttpClient _httpClient = httpClient;
        private readonly OllamaConfig _routerConfig = options.Value;

        #endregion Declarations

        /// <summary>
        /// ption>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string?> Translate(string input)
        {
            #region Declarations

            string tokenRes = string.Empty;
            string prompt = (_routerConfig.Prompt ?? string.Empty).Replace("{input}",
                input, StringComparison.OrdinalIgnoreCase);

            #endregion Declarations

            //create the ollama api client
            OllamaApiClient client = new(_httpClient)
            { SelectedModel = _routerConfig.Model };

            await foreach (GenerateResponseStream? token in client.GenerateAsync(prompt))
            {
                if (token?.Response != null)
                    tokenRes += token.Response;
            }

            return tokenRes;
        }
    }
}