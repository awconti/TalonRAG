using TalonRAG.Domain.Enums;

namespace TalonRAG.Domain.Models
{
	/// <summary>
	/// Model class representing a conversation or sequence of <see cref="Message" /> instances on behalf of the domain.
	/// </summary>
	public class Conversation
	{
		/// <summary>
		/// A list collection of <see cref="Message" />.
		/// </summary>
		public IList<Message> Messages { get; set; } = [];

		/// <summary>
		/// Adds a system message to collection of <see cref="Message" />.
		/// </summary>
		/// <param name="message">
		/// String representation of message.
		/// </param>
		public void AddSystemMessage(string message)
		{
			AddMessage(MessageAuthorRole.System, message);
		}

		/// <summary>
		/// Adds an assistant message to collection of <see cref="Message" />.
		/// </summary>
		/// <param name="message">
		/// String representation of message.
		/// </param>
		public void AddAssistantMessage(string message)
		{
			AddMessage(MessageAuthorRole.Assistant, message);
		}

		/// <summary>
		/// Adds an user message to collection of <see cref="Message" />.
		/// </summary>
		/// <param name="message">
		/// String representation of message.
		/// </param>
		public void AddUserMessage(string message)
		{
			AddMessage(MessageAuthorRole.User, message);
		}

		/// <summary>
		/// Adds a tool message to collection of <see cref="Message" />.
		/// </summary>
		/// <param name="message">
		/// String representation of message.
		/// </param>
		public void AddToolMessage(string message)
		{
			AddMessage(MessageAuthorRole.Tool, message);
		}

		private void AddMessage(MessageAuthorRole authorRole, string message)
		{
			var chatHistoryMessage = new Message
			{
				AuthorRole = authorRole,
				Content = message
			};

			Messages.Add(chatHistoryMessage);
		}
	}
}
