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
			var systemMessageRecord = 
				conversation.MessageRecords.LastOrDefault(messageRecord => messageRecord.MessageAuthorRole == MessageAuthorRole.System);

			var chatHistory = new ChatHistory(systemMessageRecord?.Content);
			foreach (var messageRecord in conversation.MessageRecords)
			{
				switch(messageRecord.MessageAuthorRole)
				{
					case MessageAuthorRole.Tool:
						chatHistory.AddMessage(AuthorRole.Tool, messageRecord.Content);
						break;
					case MessageAuthorRole.User:
						chatHistory.AddUserMessage(messageRecord.Content);
						break;
					case MessageAuthorRole.Assistant:
						chatHistory.AddAssistantMessage(messageRecord.Content);
						break;
				}

			}

			return chatHistory;
        }
	}
}
