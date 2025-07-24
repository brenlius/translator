using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using Translator.Design.Connector.Configs;

namespace Translator.Design.Connector.Clients
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="chatClients"></param>
    /// <param name="options"></param>
    public sealed class OllamaClient(ChatClients chatClients, IOptions<OllamaConfig> options)
    {
        #region Declarations

        private readonly IChatClient _chatClient = chatClients.Ollama;
        private readonly OllamaConfig _routerConfig = options.Value;

        #endregion Declarations

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string?> Translate(string input)
        {
            #region Declarations

            string prompt = (_routerConfig.Prompt ?? string.Empty).Replace("{input}",
                input, StringComparison.OrdinalIgnoreCase);

            #endregion Declarations

            ChatResponse response = await _chatClient.GetResponseAsync(prompt);
            return response.Messages.FirstOrDefault()?.Text ?? "No response.";
        }
    }
}