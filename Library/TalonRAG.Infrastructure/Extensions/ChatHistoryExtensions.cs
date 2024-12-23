using Microsoft.SemanticKernel.ChatCompletion;
using TalonRAG.Domain.Enums;
using TalonRAG.Domain.Models;

namespace TalonRAG.Infrastructure.Extensions
{
	/// <summary>
	/// Extensions tailored to <see cref="Conversation" /> domain model conversion.
	/// </summary>
	public static class ChatHistoryExtensions
	{
		/// <summary>
		/// Converts domain model to <see cref="ChatHistory" /> instance.
		/// </summary>
		/// <param name="chatHistory"></param>
		public static ChatHistory ToChatHistory(this Conversation conversation)
		{
			var systemMessage = 
				conversation.Messages.LastOrDefault(message => message.AuthorRole == MessageAuthorRole.System);

			var chatHistory = new ChatHistory(systemMessage?.Content);
			foreach (var message in conversation.Messages)
			{
				switch(message.AuthorRole)
				{
					case MessageAuthorRole.Tool:
						chatHistory.AddMessage(AuthorRole.Tool, message.Content);
						break;
					case MessageAuthorRole.User:
						chatHistory.AddUserMessage(message.Content);
						break;
					case MessageAuthorRole.Assistant:
						chatHistory.AddAssistantMessage(message.Content);
						break;
				}

			}

			return chatHistory;
        }
	}
}
