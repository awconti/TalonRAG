using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Reflection;
using TalonRAG.Application.Interfaces;
using TalonRAG.Application.Services;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Domain.Services;
using TalonRAG.Infrastructure.ConfigurationSettings;
using TalonRAG.Infrastructure.NewsAPI;
using TalonRAG.Infrastructure.Repositories;
using TalonRAG.Infrastructure.SemanticKernel;

namespace TalonRAG.Application.Extensions
{
    public static class DependencyBuilderExtensions
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
		public static void RegisterETLConsoleDependencies(this HostBuilderContext context, IServiceCollection services)
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

			services.AddScoped<IArticleEmbeddingRepository, NpgsqlArticleEmbeddingRepository>();
			services.AddScoped<IArticleEmbeddingService, ArticleEmbeddingService>();
			services.AddScoped<IEmbeddingGenerationService, HuggingFaceEmbeddingGenerationService>();
			services.AddScoped<IConsoleAppService, EtlConsoleAppService>();
		}

		/// <summary>
		/// Registers dependencies for the TalonRAG console solution.
		/// </summary>
		/// <param name="context">
		/// <see cref="HostBuilderContext" />.
		/// </param>
		/// <param name="services">
		/// <see cref="IServiceCollection"/>
		/// </param>
		public static void RegisterRagConsoleDependencies(this HostBuilderContext context, IServiceCollection services)
		{
			var databaseConfig = context.Configuration.GetSection("DatabaseConfigurationSettings");
			var chatCompletionConfig = context.Configuration.GetSection("ChatCompletionConfigurationSettings");
			var embeddingGenerationConfig = context.Configuration.GetSection("EmbeddingGenerationConfigurationSettings");
			services.Configure<DatabaseConfigurationSettings>(databaseConfig);
			services.Configure<ChatCompletionConfigurationSettings>(chatCompletionConfig);
			services.Configure<EmbeddingGenerationConfigurationSettings>(embeddingGenerationConfig);

			services.AddScoped<IArticleEmbeddingRepository, NpgsqlArticleEmbeddingRepository>();
			services.AddScoped<IMessageRepository, NpgsqlMessageRepository>();
			services.AddScoped<IConversationRepository, NpgsqlConversationRepository>();
			services.AddScoped<IArticleEmbeddingService, ArticleEmbeddingService>();
			services.AddScoped<IConversationService, ArticleConversationService>();
			services.AddScoped<IChatCompletionService, HuggingFaceChatCompletionService>();
			services.AddScoped<IEmbeddingGenerationService, HuggingFaceEmbeddingGenerationService>();
			services.AddScoped<IConsoleAppService, RagConsoleAppService>();
		}

		/// <summary>
		/// Registers dependencies for the Conversation Web API solution.
		/// </summary>
		/// <param name="builder">
		/// <see cref="IHostApplicationBuilder" />.
		/// </param>
		public static void RegisterConversationWebApiDependencies(this IHostApplicationBuilder builder)
		{
			var databaseConfig = builder.Configuration.GetSection("DatabaseConfigurationSettings");
			var chatCompletionConfig = builder.Configuration.GetSection("ChatCompletionConfigurationSettings");
			var embeddingGenerationConfig = builder.Configuration.GetSection("EmbeddingGenerationConfigurationSettings");

			var services = builder.Services;
			services.Configure<DatabaseConfigurationSettings>(databaseConfig);
			services.Configure<ChatCompletionConfigurationSettings>(chatCompletionConfig);
			services.Configure<EmbeddingGenerationConfigurationSettings>(embeddingGenerationConfig);

			services.AddScoped<IArticleEmbeddingRepository, NpgsqlArticleEmbeddingRepository>();
			services.AddScoped<IMessageRepository, NpgsqlMessageRepository>();
			services.AddScoped<IConversationRepository, NpgsqlConversationRepository>();
			services.AddScoped<IUserRepository, NpgsqlUserRepository>();
			services.AddScoped<IArticleEmbeddingService, ArticleEmbeddingService>();
			services.AddScoped<IConversationService, ArticleConversationService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IChatCompletionService, HuggingFaceChatCompletionService>();
			services.AddScoped<IEmbeddingGenerationService, HuggingFaceEmbeddingGenerationService>();
			services.AddScoped<IConversationApiService, ConversationApiService>();
			services.AddScoped<IUserApiService, UserApiService>();
		}
	}
}
