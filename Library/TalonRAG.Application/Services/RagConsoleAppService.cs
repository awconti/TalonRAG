using TalonRAG.Application.Interfaces;
using TalonRAG.Domain.Interfaces;

namespace TalonRAG.Application.Services
{
    /// <summary>
    /// RAG console application service class implementation of <see cref="IConsoleAppService" />.
    /// </summary>
	/// <param name="conversationService"> 
	/// <see cref="IConversationService" />.
	/// </param>
    public class RagConsoleAppService(IConversationService conversationService) : IConsoleAppService
	{
		private readonly IConversationService _conversationService = conversationService;

		/// <inheritdoc cref="IConsoleAppService.RunAsync" />
		public async Task RunAsync()
		{
			try
			{
				var userId = 1;
				var conversation = await _conversationService.StartConversationAsync(userId) 
					?? throw new Exception("New conversation could not be created.");

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

					conversation = await _conversationService.ContinueConversationAsync(conversation.Id, userMessage)
						?? throw new Exception("Conversation could not be continued as it could not be found.");
					var assistantMessage = conversation.Messages.Last().Content;

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