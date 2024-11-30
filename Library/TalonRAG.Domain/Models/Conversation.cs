using TalonRAG.Domain.Entities;

namespace TalonRAG.Domain.Models
{
	/// <summary>
	/// Model class representing a sequence of <see cref="MessageRecord" /> instances for a specific <see cref="Entities.ConversationRecord" /> instance on behalf of the domain.
	/// </summary>
	public class Conversation
	{
		/// <summary>
		/// Represents the persisted conversation.
		/// </summary>
		public required ConversationRecord ConversationRecord;

		/// <summary>
		/// Represents a collection of persisted messages for the conversation.
		/// </summary>
		public IList<MessageRecord> MessageRecords { get; set; } = [];
	}
}
