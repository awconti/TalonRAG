namespace TalonRAG.Application.Exceptions
{
	public class UserNotFoundException(int userId) : Exception($"User {userId} for conversation not found.")
	{
	}
}
