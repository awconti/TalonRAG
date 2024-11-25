using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TalonRAG.Application.Interfaces;
using TalonRAG.Application.Registrars;

internal class Program
{
	static async Task Main(string[] args)
	{
		using IHost host = Host.CreateDefaultBuilder(args)
			.ConfigureAppConfiguration((context, config) =>
			{
				config.AddJsonFile("./Properties/appSettings.json", optional: false, reloadOnChange: true);
			})
			.ConfigureServices(ETLConsoleDependencyRegistrar.Register)
			.Build();

		// Resolve and run the application
		var app = host.Services.GetRequiredService<IConsoleAppService>();
		await app.RunAsync();
	}
}