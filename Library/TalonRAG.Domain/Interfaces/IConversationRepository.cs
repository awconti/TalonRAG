using TalonRAG.Domain.Entities;

namespace TalonRAG.Domain.Interfaces
{
	/// <summary>
	/// Interface for repository classes seeking to implement database operations for <see cref="ConversationRecord" /> instances on behalf of the domain.
	/// </summary>
	public interface IConversationRepository
	{
		/// <summary>
		/// Persists an instance of <see cref="ConversationRecord" />.
		/// </summary>
		/// <param name="conversationRecord">
		/// Instance of <see cref="ConversationRecord" /> to create.
		/// </param>
		Task<int> InsertConversationAsync(ConversationRecord conversationRecord);

		/// <summary>
		/// Retrieves a conversation by ID.
		/// </summary>
		/// <param name="conversationId">
		/// The unique database identifier of the conversion.
		/// </param>
		Task<ConversationRecord?> GetConversationByIdAsync(int conversationId);

		/// <summary>
		/// Retrieves a list collection of conversations by user ID.
		/// </summary>
		/// <param name="userId">
		/// The unique database identifier of the user.
		/// </param>
		Task<IList<ConversationRecord>> GetConversationsByUserIdAsync(int userId);
	}
}
