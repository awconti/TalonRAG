using TalonRAG.Domain.Models;

namespace TalonRAG.Domain.Interfaces
{
	/// <summary>
	/// Interface for repository classes seeking to implement database operations for <see cref="ConversationModel" /> instances on behalf of the domain.
	/// </summary>
	public interface IConversationRepository
	{
		/// <summary>
		/// Persists an instance of <see cref="ConversationModel" />.
		/// </summary>
		/// <param name="userId" >
		/// The unique identifier of the user to create the conversation for.
		/// </param>
		Task<int> InsertConversationAsync(int userId);

		/// <summary>
		/// Retrieves a conversation by ID.
		/// </summary>
		/// <param name="conversationId">
		/// The unique identifier of the conversion.
		/// </param>
		Task<ConversationModel?> GetConversationByIdAsync(int conversationId);

		/// <summary>
		/// Retrieves a list collection of conversations by user ID.
		/// </summary>
		/// <param name="userId">
		/// The unique identifier of the user.
		/// </param>
		Task<IList<ConversationModel>> GetConversationsByUserIdAsync(int userId);
	}
}
