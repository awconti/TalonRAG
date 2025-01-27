namespace TalonRAG.Application.Exceptions
{
	public class UserNotFoundException(int userId) : Exception($"User {userId} not found.")
	{
	}
}
