using TalonRAG.Domain.Enums;

namespace TalonRAG.Infrastructure.Entities
{
	/// <summary>
	/// Represents a single persisted message in a conversation.
	/// </summary>
	public class MessageEntity
	{
		/// <summary>
		/// The unique database identifier of the message.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The unique database identifier of the conversation the message belongs to.
		/// </summary>
		public int ConversationId { get; set; }

		/// <summary>
		/// The type of message.
		/// </summary>
		public MessageType MessageType { get; set; }

		/// <summary>
		/// The content of the message.
		/// </summary>
		public string Content { get; set; } = string.Empty;

		/// <summary>
		/// The date and time (UTC) which message was created.
		/// </summary>
		public DateTime CreateDate { get; set; }
	}
}
