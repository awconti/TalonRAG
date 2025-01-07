using TalonRAG.Domain.Enums;

namespace TalonRAG.Application.DataTransferObjects
{
	/// <summary>
	/// DTO for representing a particular message within a conversation.
	/// </summary>
	public class MessageDto
	{
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
