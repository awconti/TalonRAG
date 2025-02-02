namespace TalonRAG.Application.Exceptions
{
	/// <summary>
	/// Custom exception class thrown when initializionizing a new conversation is unsuccessful.
	/// </summary>
	/// <param name="userId">
	/// The unique identifier of the user for the unsuccessful conversation initialization. 
	/// </param>
	public class ConversationInitializationApiException(int userId) : Exception($"Conversation initialization for user {userId} unsuccessful.")
	{ }
}
