namespace TalonRAG.Application.Exceptions
{
	public class ConversationInitializationException(int userId) : Exception($"Conversation initialization for user {userId} unsuccessful.")
	{
	}
}
