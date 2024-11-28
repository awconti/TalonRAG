namespace TalonRAG.Domain.Entities
{
	/// <summary>
	/// Represents a persisted conversation which serves as a reference to a sequence of messages.
	/// </summary>
	public class ConversationRecord
	{
		/// <summary>
		/// The unique database identifier of the conversation.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The unique database identifier of the user who initiated the conversation.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// The date and time (UTC) which conversation was created.
		/// </summary>
		public DateTime CreateDate { get; set; }
	}
}
