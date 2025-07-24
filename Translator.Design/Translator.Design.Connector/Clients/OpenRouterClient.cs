using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Translator.Design.Connector.Configs;

namespace Translator.Design.Connector.Clients
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="options"></param>
    public sealed class OpenRouterClient(HttpClient httpClient, IOptions<OpenRouterConfig> options)
    {
        #region Declarations

        private readonly HttpClient _httpClient = httpClient;
        private readonly OpenRouterConfig _routerConfig = options.Value;

        #endregion Declarations

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string?> Translate(string input)
        {
            var payload = new
            {
                model = _routerConfig.Model,
                messages = new[]
                    {
                    new { role = "system", content = _routerConfig.Prompt },
                    new { role = "user", content = input }
                }
            };

            HttpResponseMessage response = await _httpClient.PostAsync("api/v1/chat/completions",
                new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new Exception($"OpenRouter Error: {response.StatusCode}\n{await response.Content.ReadAsStringAsync()}");

            using JsonDocument jsonDocument = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

            return jsonDocument.RootElement.GetProperty("choices")[0]
                .GetProperty("message").GetProperty("content").GetString();
        }

        #endregion Methods
    }
}
