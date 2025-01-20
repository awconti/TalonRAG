namespace TalonRAG.Application.Exceptions
{
	public class ConversationNotFoundException(int conversationId) : Exception($"Conversation {conversationId} not found.")
	{ }
}
