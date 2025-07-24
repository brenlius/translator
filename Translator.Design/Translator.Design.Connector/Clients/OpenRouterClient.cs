using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using Translator.Design.Connector.Configs;

namespace Translator.Design.Connector.Clients
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="chatClient"></param>
    /// <param name="options"></param>
    public sealed class OpenRouterClient(IChatClient chatClient, IOptions<OpenRouterConfig> options)
    {
        #region Declarations

        private readonly IChatClient _chatClient = chatClient;
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
                new(Microsoft.Extensions.AI.ChatRole.System, _routerConfig.Prompt),
                new(Microsoft.Extensions.AI.ChatRole.User, input)
            ];

            ChatResponse response = await _chatClient.GetResponseAsync(messages);
            return response.Messages.FirstOrDefault()?.Text ?? "No response";
        }

        #endregion Methods
    }
}
