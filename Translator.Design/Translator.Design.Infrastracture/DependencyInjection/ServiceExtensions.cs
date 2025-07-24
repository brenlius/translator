using System.ClientModel;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using Translator.Design.Application.IRepositories;
using Translator.Design.Connector.Clients;
using Translator.Design.Connector.Configs;
using Translator.Design.Infrastracture.Repositories;

namespace Translator.Design.Infrastracture.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterExternalServices(this IServiceCollection services, IConfiguration config)
        {
            // Ollama
            services.Configure<OllamaConfig>(config.GetSection("Ollama"));
            services.AddHttpClient<OllamaClient>((sp, client) =>
            {
                OllamaConfig ollamaConfig = sp.GetRequiredService<IOptions<OllamaConfig>>().Value;
                client.BaseAddress = new Uri(ollamaConfig.BaseAddress);
            });

            // Open Router
            services.Configure<OpenRouterConfig>(config.GetSection("OpenRouter"));
            services.AddChatClient(serviceProvider =>
            {
                OpenRouterConfig openRouterConfig = serviceProvider.GetRequiredService<IOptions<OpenRouterConfig>>().Value;
                return new ChatClient(openRouterConfig.Model, new ApiKeyCredential(openRouterConfig.ApiKey), new()
                { Endpoint = new Uri(openRouterConfig.BaseAddress) }).AsIChatClient();
            });
            services.AddEmbeddingGenerator(serviceProvider =>
            {
                OpenRouterConfig openRouterConfig = serviceProvider.GetRequiredService<IOptions<OpenRouterConfig>>().Value;
                return new OpenAI.Embeddings.EmbeddingClient(openRouterConfig.Model, openRouterConfig.ApiKey)
                    .AsIEmbeddingGenerator();
            });

            services.AddScoped<ITranslateRepository, TranslateRepository>();
            services.AddScoped<OpenRouterClient>();

            return services;
        }
    }
}