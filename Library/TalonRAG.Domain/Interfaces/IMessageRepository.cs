using TalonRAG.Domain.Models;

namespace TalonRAG.Domain.Interfaces
{
	/// <summary>
	/// Interface for repository classes seeking to implement database operations for <see cref="MessageModel" /> instances on behalf of the domain.
	/// </summary>
	public interface IMessageRepository
	{
		/// <summary>
		/// Persists an instance of <see cref="MessageModel" />.
		/// </summary>
		/// <param name="conversationId">
		/// The unique database identifier of the conversation the message belongs to.
		/// </param>
		/// <param name="messageEntity">
		/// An instance of <see cref="MessageModel" /> to persist.
		/// </param>
		Task<int> InsertMessageAsync(MessageModel messageModel);

		/// <summary>
		/// Retrieves a list collection of messages by conversation ID.
		/// </summary>
		/// <param name="conversationId">
		/// The unique database identifier of the conversation.
		/// </param>
		Task<IList<MessageModel>> GetMessagesByConversationIdAsync(int conversationId);
	}
}
