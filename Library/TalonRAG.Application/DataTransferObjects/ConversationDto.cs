namespace TalonRAG.Application.DataTransferObjects
{
	/// <summary>
	/// DTO representing a conversation for a particular user.
	/// </summary>
	public class ConversationDto
	{
		/// <summary>
		/// The unique database identifier of the conversation.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The unique database identifier of the user.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// The date and time which the conversation was started.
		/// </summary>
		public DateTime CreateDate { get; set; }

		/// <summary>
		/// A collection of messages associated with this conversation.
		/// </summary>
		public IList<MessageDto>? Messages { get; set; }
	}
}
