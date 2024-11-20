using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TalonRAG.Application.Services;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Infrastructure.ConfigurationSettings;
using TalonRAG.Infrastructure.Repositories;
using TalonRAG.Infrastructure.SemanticKernel.ChatCompletion;
using TalonRAG.Infrastructure.SemanticKernel.Embedding;

namespace TalonRAG.Application.Registrars
{
	/// <summary>
	/// Class dedicated to registering dependencies for the TalonRAG ETL console solution.
	/// </summary>
	public static class RAGConsoleDependencyRegistrar
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
			var chatCompletorConfig = context.Configuration.GetSection("ChatCompletorConfigurationSettings");
			var embeddingGeneratorConfig = context.Configuration.GetSection("EmbeddingGeneratorConfigurationSettings");
			services.Configure<DatabaseConfigurationSettings>(databaseConfig);
			services.Configure<ChatCompletorConfigurationSettings>(chatCompletorConfig);
			services.Configure<EmbeddingGeneratorConfigurationSettings>(embeddingGeneratorConfig);

			services.AddTransient<IEmbeddingRepository, NpgsqlArticleEmbeddingRepository>();
			services.AddTransient<IChatCompletor, HuggingFaceChatCompletor>();
			services.AddTransient<IEmbeddingGenerator, HuggingFaceEmbeddingGenerator>();
			services.AddTransient<IConsoleAppService, RAGConsoleAppService>();
		}
	}
}
