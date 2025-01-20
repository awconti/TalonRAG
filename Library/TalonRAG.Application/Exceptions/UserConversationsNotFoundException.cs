namespace TalonRAG.Application.Exceptions
{
	public class UserConversationsNotFoundException(int userId) : Exception($"Conversations for user {userId} not found.")
	{ }
}
