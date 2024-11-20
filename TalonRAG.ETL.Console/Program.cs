using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Infrastructure.ConfigurationSettings;
using TalonRAG.Infrastructure.Repositories;
using TalonRAG.Infrastructure.SemanticKernel.Embedding;

internal class Program
{
	static async Task Main(string[] args)
	{
		using IHost host = Host.CreateDefaultBuilder(args)
			.ConfigureAppConfiguration((context, config) =>
			{
				config.AddJsonFile("./Properties/appSettings.json", optional: false, reloadOnChange: true);
			})
			.ConfigureServices((context, services) =>
			{
				var databaseConfig = context.Configuration.GetSection("DatabaseConfigurationSettings");
				var embeddingGeneratorConfig = context.Configuration.GetSection("EmbeddingGeneratorConfigurationSettings");
				services.Configure<DatabaseConfigurationSettings>(databaseConfig);
				services.Configure<EmbeddingGeneratorConfigurationSettings>(embeddingGeneratorConfig);

				services.AddTransient<IEmbeddingRepository, NpgsqlArticleEmbeddingRepository>();
				services.AddTransient<IEmbeddingGenerator, HuggingFaceEmbeddingGenerator>();
				services.AddTransient<ETLConsoleAppService>();
			})
			.Build();

		// Resolve and run the application
		var app = host.Services.GetRequiredService<ETLConsoleAppService>();
		await app.RunAsync();
	}
}