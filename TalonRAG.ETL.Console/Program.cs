using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TalonRAG.Common.Configuration;
using TalonRAG.Common.Embedding;
using TalonRAG.Common.Persistence.Repository;

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

				services.AddSingleton<IEmbeddingRepository, NpgsqlArticleEmbeddingRepository>();
				services.AddSingleton<IEmbeddingGenerator, HuggingFaceEmbeddingGenerator>();
				services.AddSingleton<ETLConsoleService>();
			})
			.Build();

		// Resolve and run the application
		var app = host.Services.GetRequiredService<ETLConsoleService>();
		await app.RunAsync();
	}
}