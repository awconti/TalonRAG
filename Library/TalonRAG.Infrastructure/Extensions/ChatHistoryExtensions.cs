using Microsoft.SemanticKernel.ChatCompletion;

namespace TalonRAG.Infrastructure.Extensions
{
	/// <summary>
	/// Extensions tailored to <see cref="Domain.Models.ChatHistory" /> domain model conversion.
	/// </summary>
	public static class ChatHistoryExtensions
	{
		/// <summary>
		/// Converts domain model to <see cref="ChatHistory" /> instance.
		/// </summary>
		/// <param name="chatHistory"></param>
		public static ChatHistory ToKernelChatHistory(this Domain.Models.ChatHistory chatHistory)
		{
			var systemMessage = 
				chatHistory.Messages.LastOrDefault(message => message.AuthorRole == Domain.Enums.AuthorRole.System);

			var kernelChatHistory = new ChatHistory(systemMessage?.Content);
			foreach (var message in chatHistory.Messages)
			{
				switch(message.AuthorRole)
				{
					case Domain.Enums.AuthorRole.Tool:
						kernelChatHistory.AddMessage(AuthorRole.Tool, message.Content);
						break;
					case Domain.Enums.AuthorRole.User:
						kernelChatHistory.AddUserMessage(message.Content);
						break;
					case Domain.Enums.AuthorRole.Assistant:
						kernelChatHistory.AddAssistantMessage(message.Content);
						break;
				}

			}

			return kernelChatHistory;
        }
	}
}
