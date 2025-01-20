namespace TalonRAG.Application.DataTransferObjects
{
	/// <summary>
	/// DTO for representing a particular message within a conversation.
	/// </summary>
	public class MessageDto
	{
		/// <summary>
		/// Description of <see cref="MessageType" />.
		/// </summary>
		public string MessageType { get; set; } = string.Empty;

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
