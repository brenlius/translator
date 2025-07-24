using System.ClientModel;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OllamaSharp;
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
            // Config bindings
            services.Configure<OllamaConfig>(config.GetSection("Ollama"));
            services.Configure<OpenRouterConfig>(config.GetSection("OpenRouter"));

            // Register ChatClients manually
            services.AddScoped<ChatClients>(serviceProvider =>
            {
                // Get configurations
                OllamaConfig ollamaConfig = serviceProvider.GetRequiredService<IOptions<OllamaConfig>>().Value;
                OpenRouterConfig openRouterConfig = serviceProvider.GetRequiredService<IOptions<OpenRouterConfig>>().Value;

                // Create chat clients
                IChatClient ollama = new OllamaApiClient(ollamaConfig.BaseAddress, ollamaConfig.Model);
                IChatClient openRouter = new ChatClient(openRouterConfig.Model, new ApiKeyCredential(openRouterConfig.ApiKey), new()
                { Endpoint = new Uri(openRouterConfig.BaseAddress) }).AsIChatClient();

                return new ChatClients
                {
                    Ollama = ollama,
                    OpenRouter = openRouter
                };
            });

            // Add embedding OpenRouter client
            services.AddEmbeddingGenerator(sp =>
            {
                OpenRouterConfig openRouterConfig = sp.GetRequiredService<IOptions<OpenRouterConfig>>().Value;
                return new OpenAI.Embeddings.EmbeddingClient(openRouterConfig.Model, openRouterConfig.ApiKey)
                    .AsIEmbeddingGenerator();
            });

            services.AddScoped<ITranslateRepository, TranslateRepository>();
            services.AddScoped<OllamaClient>();
            services.AddScoped<OpenRouterClient>();

            return services;
        }
    }
}