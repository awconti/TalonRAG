using TalonRAG.Domain.Models;

namespace TalonRAG.Domain.Interfaces
{
	/// <summary>
	/// Interface for classes seeking to manage a conversation on behalf of the domain.
	/// </summary>
	public interface IConversationService
	{
		/// <summary>
		/// Retrieves a conversation by id.
		/// </summary>
		/// <param name="id">
		/// The unique database identifier of the conversation.
		/// </param>
		Task<Conversation?> GetConversationByIdAsync(int id);

		/// <summary>
		/// Creates a conversation for a specific user.
		/// </summary>
		/// <param name="userId">
		/// The unique database identifier of the user.
		/// </param>
		Task<Conversation?> StartConversationAsync(int userId);

		/// <summary>
		/// Continues a specific conversation on behalf of the user.
		/// </summary>
		/// <param name="conversationId">
		/// The unique database identifier of the conversation.
		/// </param>
		/// <param name="userMessageContent">
		/// The content of the user's message.
		/// </param>
		Task<Conversation?> ContinueConversationAsync(int conversationId, string userMessageContent);
	}
}
