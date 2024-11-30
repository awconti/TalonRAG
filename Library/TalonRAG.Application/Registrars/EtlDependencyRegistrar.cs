using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Reflection;
using TalonRAG.Application.Services;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Domain.Services;
using TalonRAG.Infrastructure.ConfigurationSettings;
using TalonRAG.Infrastructure.NewsAPI;
using TalonRAG.Infrastructure.Repositories;
using TalonRAG.Infrastructure.SemanticKernel;

namespace TalonRAG.Application.Registrars
{
    /// <summary>
    /// Class dedicated to registering dependencies for the TalonRAG ETL console solution.
    /// </summary>
    public static class EtlDependencyRegistrar
	{
		/// <summary>
		/// Registers dependencies for the TalonRAG ETL console solution.
		/// </summary>
		/// <param name="context">
		/// <see cref="HostBuilderContext" />.
		/// </param>
		/// <param name="services">
		/// <see cref="IServiceCollection"/>
		/// </param>
		public static void Register(HostBuilderContext context, IServiceCollection services)
		{
			var databaseConfig = context.Configuration.GetSection("DatabaseConfigurationSettings");
			var embeddingGenerationConfig = context.Configuration.GetSection("EmbeddingGenerationConfigurationSettings");
			var newsApiConfig = context.Configuration.GetSection("NewsApiConfigurationSettings");
			services.Configure<DatabaseConfigurationSettings>(databaseConfig);
			services.Configure<EmbeddingGenerationConfigurationSettings>(embeddingGenerationConfig);
			services.Configure<NewsApiConfigurationSettings>(newsApiConfig);

			services.AddHttpClient<IExternalArticleApiClient, NewsApiClient>((serviceProvider, client) =>
			{
				var settings = serviceProvider.GetRequiredService<IOptions<NewsApiConfigurationSettings>>().Value;
				client.BaseAddress = new Uri(settings.BaseUrl ?? string.Empty);
				client.Timeout = TimeSpan.FromSeconds(30);
				client.DefaultRequestHeaders.Add("Authorization", settings.ApiKey);
				client.DefaultRequestHeaders.Add("User-Agent", Assembly.GetExecutingAssembly().GetName().Name);
			});

			services.AddTransient<IArticleEmbeddingRepository, NpgsqlArticleEmbeddingRepository>();
			services.AddTransient<IArticleEmbeddingService, ArticleEmbeddingService>();
			services.AddTransient<IEmbeddingGenerationService, HuggingFaceEmbeddingGenerationService>();
			services.AddTransient<IConsoleAppService, EtlConsoleAppService>();
		}
	}
}
