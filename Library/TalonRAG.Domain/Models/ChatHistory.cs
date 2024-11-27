using TalonRAG.Domain.Enums;

namespace TalonRAG.Domain.Models
{
	/// <summary>
	/// Model class for providing the history of a chat on behalf of the domain.
	/// </summary>
	public class ChatHistory
	{
		/// <summary>
		/// A list collection of <see cref="ChatHistoryMessage" />.
		/// </summary>
		public IList<ChatHistoryMessage> Messages { get; set; } = [];

		/// <summary>
		/// Adds a system message to collection of <see cref="ChatHistoryMessage" />.
		/// </summary>
		/// <param name="message">
		/// String representation of message.
		/// </param>
		public void AddSystemMessage(string message)
		{
			AddMessage(AuthorRole.System, message);
		}

		/// <summary>
		/// Adds an assistant message to collection of <see cref="ChatHistoryMessage" />.
		/// </summary>
		/// <param name="message">
		/// String representation of message.
		/// </param>
		public void AddAssistantMessage(string message)
		{
			AddMessage(AuthorRole.Assistant, message);
		}

		/// <summary>
		/// Adds an user message to collection of <see cref="ChatHistoryMessage" />.
		/// </summary>
		/// <param name="message">
		/// String representation of message.
		/// </param>
		public void AddUserMessage(string message)
		{
			AddMessage(AuthorRole.User, message);
		}

		/// <summary>
		/// Adds a tool message to collection of <see cref="ChatHistoryMessage" />.
		/// </summary>
		/// <param name="message">
		/// String representation of message.
		/// </param>
		public void AddToolMessage(string message)
		{
			AddMessage(AuthorRole.Tool, message);
		}

		private void AddMessage(AuthorRole authorRole, string message)
		{
			var chatHistoryMessage = new ChatHistoryMessage
			{
				AuthorRole = authorRole,
				Content = message
			};

			Messages.Add(chatHistoryMessage);
		}
	}
}
