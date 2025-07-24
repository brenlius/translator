using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
        /// /
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
            services.AddHttpClient<OpenRouterClient>((sp, client) =>
            {
                OpenRouterConfig routerConfig = sp.GetRequiredService<IOptions<OpenRouterConfig>>().Value;
                client.BaseAddress = new Uri(routerConfig.BaseAddress);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", routerConfig.ApiKey);
                client.DefaultRequestHeaders.Add("HTTP-Referer", routerConfig.Referer);
                client.DefaultRequestHeaders.Add("X-Title", routerConfig.Title);
            });

            services.AddSingleton<ITranslateRepository, TranslateRepository>();

            return services;
        }
    }
}
