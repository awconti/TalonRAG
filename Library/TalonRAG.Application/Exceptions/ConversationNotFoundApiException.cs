namespace TalonRAG.Application.Exceptions
{
	/// <summary>
	/// Custom exception class thrown when a conversation could not be found.
	/// </summary>
	/// <param name="conversationId">
	/// The unique identifier of the conversation that could not be found.
	/// </param>
	public class ConversationNotFoundApiException(int conversationId) : Exception($"Conversation {conversationId} not found.")
	{ }
}
