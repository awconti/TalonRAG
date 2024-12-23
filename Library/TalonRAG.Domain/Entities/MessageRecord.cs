using TalonRAG.Domain.Enums;

namespace TalonRAG.Domain.Entities
{
	/// <summary>
	/// Represents a single persisted message in a conversation.
	/// </summary>
	public class MessageRecord
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
		/// The role of the author of... the message. 
		/// </summary>
		public MessageAuthorRole MessageAuthorRole { get; set; }

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
