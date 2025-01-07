using Microsoft.SemanticKernel.ChatCompletion;
using TalonRAG.Domain.Enums;
using TalonRAG.Domain.Models;

namespace TalonRAG.Infrastructure.Extensions
{
	/// <summary>
	/// Extensions tailored to <see cref="ConversationModel" /> conversion.
	/// </summary>
	public static class ConversationModelExtensions
	{
		/// <summary>
		/// Converts domain model to <see cref="ChatHistory" /> instance.
		/// </summary>
		/// <param name="chatHistory"></param>
		public static ChatHistory ToChatHistory(this ConversationModel conversation)
		{
			var systemMessage = 
				conversation.Messages.LastOrDefault(message => message.MessageType == MessageType.System);

			var chatHistory = new ChatHistory(systemMessage?.Content);
			foreach (var message in conversation.Messages)
			{
				switch (message.MessageType)
				{
					case MessageType.Tool:
						chatHistory.AddMessage(AuthorRole.Tool, message.Content);
						break;
					case MessageType.User:
						chatHistory.AddUserMessage(message.Content);
						break;
					case MessageType.Assistant:
						chatHistory.AddAssistantMessage(message.Content);
						break;
				}

			}

			return chatHistory;
        }
	}
}
