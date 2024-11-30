using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TalonRAG.Application.Services;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Domain.Services;
using TalonRAG.Infrastructure.ConfigurationSettings;
using TalonRAG.Infrastructure.Repositories;
using TalonRAG.Infrastructure.SemanticKernel;

namespace TalonRAG.Application.Registrars
{
    /// <summary>
    /// Class dedicated to registering dependencies for the TalonRAG ETL console solution.
    /// </summary>
    public static class RagDependencyRegistrar
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
			var chatCompletionConfig = context.Configuration.GetSection("ChatCompletionConfigurationSettings");
			var embeddingGenerationConfig = context.Configuration.GetSection("EmbeddingGenerationConfigurationSettings");
			services.Configure<DatabaseConfigurationSettings>(databaseConfig);
			services.Configure<ChatCompletionConfigurationSettings>(chatCompletionConfig);
			services.Configure<EmbeddingGenerationConfigurationSettings>(embeddingGenerationConfig);

			services.AddTransient<IArticleEmbeddingRepository, NpgsqlArticleEmbeddingRepository>();
			services.AddTransient<IMessageRepository, NpgsqlMessageRepository>();
			services.AddTransient<IConversationRepository, NpgsqlConversationRepository>();
			services.AddTransient<IArticleEmbeddingService, ArticleEmbeddingService>();
			services.AddTransient<IConversationManagerService, ConversationManagerService>();
			services.AddTransient<IChatCompletionService, HuggingFaceChatCompletionService>();
			services.AddTransient<IEmbeddingGenerationService, HuggingFaceEmbeddingGenerationService>();
			services.AddTransient<IConsoleAppService, RagAppService>();
		}
	}
}
