using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TalonRAG.Application.Services;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Infrastructure.ConfigurationSettings;
using TalonRAG.Infrastructure.NewsAPI;
using TalonRAG.Infrastructure.Repositories;
using TalonRAG.Infrastructure.SemanticKernel;

namespace TalonRAG.Application.Registrars
{
    /// <summary>
    /// Class dedicated to registering dependencies for the TalonRAG ETL console solution.
    /// </summary>
    public static class ETLConsoleDependencyRegistrar
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
			var embeddingGeneratorConfig = context.Configuration.GetSection("EmbeddingGeneratorConfigurationSettings");
			var newsApiConfig = context.Configuration.GetSection("NewsApiConfigurationSettings");
			services.Configure<DatabaseConfigurationSettings>(databaseConfig);
			services.Configure<EmbeddingGeneratorConfigurationSettings>(embeddingGeneratorConfig);
			services.Configure<NewsApiConfigurationSettings>(newsApiConfig);

			services.AddHttpClient<INewsApiClient, NewsApiClient>((serviceProvider, client) =>
			{
				var settings = serviceProvider.GetRequiredService<IOptions<NewsApiConfigurationSettings>>().Value;
				client.BaseAddress = new Uri(settings.BaseUrl ?? "");
				client.DefaultRequestHeaders.Add("Authorization", $"Bearer {settings.ApiKey}");
			});

			services.AddTransient<IArticleEmbeddingRepository, NpgsqlArticleEmbeddingRepository>();
			services.AddTransient<IEmbeddingGenerator, HuggingFaceEmbeddingGenerator>();
			services.AddTransient<IConsoleAppService, ETLConsoleAppService>();
		}
	}
}
