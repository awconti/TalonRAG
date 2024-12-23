namespace TalonRAG.Domain.Models
{
	/// <summary>
	/// Model class representing a conversation on behalf of the domain.
	/// </summary>
	public class Conversation
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
		/// Collection of messages which are a part of the conversation.
		/// </summary>
		public List<Message> Messages { get; private set; } = [];

		/// <summary>
		/// Adds a message to the collection of messages in the conversation.
		/// </summary>
		/// <param name="messages">
		/// <see cref="Message" />.
		/// </param>
		public void AddMessages(IList<Message> messages)
		{
			Messages.AddRange(messages);
		}

		/// <summary>
		/// Sets the message collection in the conversation.
		/// </summary>
		/// <param name="messages">
		/// A collection of <see cref="Message" /> instances.
		/// </param>
		public void SetMessages(IList<Message> messages)
		{
			Messages = (List<Message>) messages;
		}
	}
}
