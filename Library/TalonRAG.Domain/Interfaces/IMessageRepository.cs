using TalonRAG.Domain.Entities;

namespace TalonRAG.Domain.Interfaces
{
	/// <summary>
	/// Interface for repository classes seeking to implement database operations for <see cref="MessageRecord" /> instances on behalf of the domain.
	/// </summary>
	public interface IMessageRepository
	{
		/// <summary>
		/// Inserts a list collection of <see cref="MessageRecord" />.
		/// </summary>
		/// <param name="messages">
		/// List collection of <see cref="MessageRecord" />.
		/// </param>
		Task InsertMessagesAsync(IList<MessageRecord> messages);

		/// <summary>
		/// Retrieves a list collection of messages by conversation ID.
		/// </summary>
		/// <param name="conversationId">
		/// The unique database identifier of the conversation.
		/// </param>
		Task<IList<MessageRecord>> GetMessagesByConversationIdAsync(int conversationId);
	}
}
