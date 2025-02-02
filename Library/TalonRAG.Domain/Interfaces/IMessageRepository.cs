using TalonRAG.Domain.Models;

namespace TalonRAG.Domain.Interfaces
{
	/// <summary>
	/// Interface for repository classes seeking to implement database operations for <see cref="MessageModel" /> instances on behalf of the domain.
	/// </summary>
	public interface IMessageRepository
	{
		/// <summary>
		/// Retrieves a list collection of messages by conversation ID.
		/// </summary>
		/// <param name="conversationId">
		/// The unique database identifier of the conversation.
		/// </param>
		Task<IList<MessageModel>> GetMessagesByConversationIdAsync(int conversationId);

		/// <summary>
		/// Retrieves a list collection of messages by conversation ID.
		/// </summary>
		/// <param name="conversationIds">
		/// The unique database identifiers of conversations to retrieve messages for.
		/// </param>
		Task<IList<MessageModel>> GetMessagesByConversationIdsAsync(int[] conversationIds);

		/// <summary>
		/// Retrieves a list collection of messages by conversation ID.
		/// </summary>
		/// <param name="conversationIds">
		/// The unique database identifiers of conversations to retrieve messages for.
		/// </param>
		Task<IList<MessageModel>> GetLastMessagesByConversationIdsAsync(int[] conversationIds);

		/// <summary>
		/// Persists an instance of <see cref="MessageModel" />.
		/// </summary>
		/// <param name="messageModel">
		/// An instance of <see cref="MessageModel" /> to persist.
		/// </param>
		Task<int> InsertMessageAsync(MessageModel messageModel);

		/// <summary>
		/// Removes all messages by conversation ID.
		/// </summary>
		/// <param name="conversationId">
		/// The unique identifier of the conversation.
		/// </param>
		Task<int> DeleteMessagesByConversationIdAsync(int conversationId);
	}
}
