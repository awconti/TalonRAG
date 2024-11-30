using TalonRAG.Domain.Interfaces;

namespace TalonRAG.Application.Services
{
    /// <summary>
    /// RAG console application service class implementation of <see cref="IConsoleAppService" />.
    /// </summary>
	/// <param name="conversationManagerService"> 
	/// <see cref="IConversationManagerService" />.
	/// </param>
    public class RagConsoleAppService(IConversationManagerService conversationManagerService) : IConsoleAppService
	{
		private readonly IConversationManagerService _conversationManagerService = conversationManagerService;

		/// <inheritdoc cref="IConsoleAppService.RunAsync" />
		public async Task RunAsync()
		{
			try
			{
				var userId = 1;
				var conversationId = await _conversationManagerService.StartConversationAsync(userId);

				while (true)
				{

					Console.Write("You: ");
					string? userMessage = Console.ReadLine();

					Console.ForegroundColor = ConsoleColor.DarkGreen;

					if (userMessage == null || userMessage.Equals("exit", StringComparison.OrdinalIgnoreCase))
					{
						Console.WriteLine("TalonRAG: Goodbye!");
						break;
					}

					var assistantMessage = await _conversationManagerService.ContinueConversationAsync(conversationId, userMessage);

					Console.WriteLine($"TalonRAG: {assistantMessage}");

					Console.ResetColor();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"TalonRAG: Encountered an exception - {ex.Message}");
			}
			finally
			{
				Console.ResetColor();
			}
		}
	}
}