using Microsoft.Extensions.AI;

namespace Translator.Design.Connector.Clients
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ChatClients
    {
        /// <summary>
        /// 
        /// </summary>
        public required IChatClient Ollama { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public required IChatClient OpenRouter { get; set; }
    }
}
