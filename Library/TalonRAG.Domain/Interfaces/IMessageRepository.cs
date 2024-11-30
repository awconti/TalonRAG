using TalonRAG.Domain.Entities;
using TalonRAG.Domain.Enums;

namespace TalonRAG.Domain.Interfaces
{
	/// <summary>
	/// Interface for repository classes seeking to implement database operations for <see cref="MessageRecord" /> instances on behalf of the domain.
	/// </summary>
	public interface IMessageRepository
	{
		/// <summary>
		/// Persists an instance of <see cref="MessageRecord" />.
		/// </summary>
		/// <param name="conversationId">
		/// The unique database identifier of the conversation the message belongs to.
		/// </param>
		/// <param name="authorRole">
		/// The role of the author of this message.
		/// </param>
		/// <param name="content">
		/// The content of the message.
		/// </param>
		Task<int> InsertMessageAsync(MessageRecord messageRecord);

		/// <summary>
		/// Retrieves a list collection of messages by conversation ID.
		/// </summary>
		/// <param name="conversationId">
		/// The unique database identifier of the conversation.
		/// </param>
		Task<IList<MessageRecord>> GetMessagesByConversationIdAsync(int conversationId);
	}
}
