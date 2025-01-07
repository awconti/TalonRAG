using TalonRAG.Domain.Enums;

namespace TalonRAG.Domain.Models
{
	/// <summary>
	/// Model representing a message within a conversation on behalf of the domain.
	/// </summary>
	public class MessageModel
	{
		/// <summary>
		/// The unique database identifier for the message.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The unique identifier for the conversation this message is a part of.
		/// </summary>
		public int ConversationId { get; set; }

		/// <summary>
		/// <see cref="MessageType" />.
		/// </summary>
		public MessageType MessageType { get; set; }

		/// <summary>
		/// The content of the message.
		/// </summary>
		public string Content { get; set; } = string.Empty;

		/// <summary>
		/// The date at which the message was created.
		/// </summary>
		public DateTime CreateDate { get; set; }
	}
}
