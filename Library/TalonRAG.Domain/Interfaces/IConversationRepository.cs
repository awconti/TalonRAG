using TalonRAG.Domain.Entities;

namespace TalonRAG.Domain.Interfaces
{
	/// <summary>
	/// Interface for repository classes seeking to implement database operations for <see cref="ConversationRecord" /> instances on behalf of the domain.
	/// </summary>
	public interface IConversationRepository
	{
		/// <summary>
		/// Inserts a list collection of <see cref="ConversationRecord" />.
		/// </summary>
		/// <param name="conversations">
		/// List collection of <see cref="ConversationRecord" />.
		/// </param>
		Task InsertConversationsAsync(IList<ConversationRecord> conversations);

		/// <summary>
		/// Retrieves a list collection of conversations by user ID.
		/// </summary>
		/// <param name="userId">
		/// The unique database identifier of the user.
		/// </param>
		Task<IList<ConversationRecord>> GetConversationsByUserIdAsync(int userId);
	}
}
