using TalonRAG.Domain.Models;

namespace TalonRAG.Domain.Interfaces
{
	/// <summary>
	/// Interface for classes seeking to manage a conversation on behalf of the domain.
	/// </summary>
	public interface IConversationService
	{
		/// <summary>
		/// Retrieves a conversation by ID.
		/// </summary>
		/// <param name="conversationId">
		/// The unique database identifier of the conversation.
		/// </param>
		Task<ConversationModel?> GetConversationByIdAsync(int conversationId);

		/// <summary>
		/// Creates a conversation for a specific user.
		/// </summary>
		/// <param name="userId">
		/// The unique database identifier of the user.
		/// </param>
		Task<ConversationModel?> StartConversationAsync(int userId);

		/// <summary>
		/// Continues a specific conversation on behalf of the user.
		/// </summary>
		/// <param name="conversationId">
		/// The unique database identifier of the conversation.
		/// </param>
		/// <param name="userMessageContent">
		/// The content of the user's message.
		/// </param>
		Task<ConversationModel?> ContinueConversationAsync(int conversationId, string userMessageContent);

		/// <summary>
		/// Retrieves a conversations by user ID.
		/// </summary>
		/// <param name="userId">
		/// The unique identifier of the user to retrieve conversations for.
		/// </param>
		Task<IList<ConversationModel>?> GetConversationsByUserIdAsync(int userId);
	}
}
