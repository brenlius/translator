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
    public sealed class OpenRouterClient(ChatClients chatClients, IOptions<OpenRouterConfig> options)
    {
        #region Declarations

        private readonly IChatClient _chatClient = chatClients.OpenRouter;
        private readonly OpenRouterConfig _routerConfig = options.Value;

        #endregion Declarations

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string?> Translate(string input)
        {
            List<ChatMessage> messages =
            [
                new(ChatRole.System, _routerConfig.Prompt),
                new(ChatRole.User, input)
            ];

            ChatResponse response = await _chatClient.GetResponseAsync(messages);
            return response.Messages.FirstOrDefault()?.Text ?? "No response.";
        }

        #endregion Methods
    }
}
