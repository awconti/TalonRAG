namespace TalonRAG.Application.Exceptions
{
	/// <summary>
	/// Custom exception class thrown when conversations for a user could not be found.
	/// </summary>
	/// <param name="userId">
	/// The unique identifier of the user for the conversations which could not be found.
	/// </param>
	public class UserConversationsNotFoundApiException(int userId) : Exception($"Conversations for user {userId} not found.")
	{ }
}
