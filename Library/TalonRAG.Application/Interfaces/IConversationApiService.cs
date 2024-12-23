using TalonRAG.Application.DataTransferObjects;

namespace TalonRAG.Application.Interfaces
{
	/// <summary>
	/// Interface for conversations seeking to implement conversation functionality via WebAPI. 
	/// </summary>
	public interface IConversationApiService
	{
		/// <summary>
		/// Retrieves conversation by ID.
		/// </summary>
		/// <param name="id">
		/// Unique database identifier of the conversation.
		/// </param>
		Task<ConversationDto?> GetConversationByIdAsync(int id);

		/// <summary>
		/// Initiates a new conversation.
		/// </summary>
		/// <param name="request">
		/// <see cref="NewConversationRequest"/>.
		/// </param>
		Task<ConversationDto?> AddNewConversationAsync(NewConversationRequest request);

		/// <summary>
		/// Updates an existing conversation.
		/// </summary>
		/// <param name="conversationId">
		/// Unique database identifier of the conversation.
		/// </param>
		/// <param name="request">
		/// <see cref="UpdateConversationRequest"/>.
		/// </param>
		Task<ConversationDto?> UpdateConversationAsync(int conversationId, UpdateConversationRequest request);
	}
}
