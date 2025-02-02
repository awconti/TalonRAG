namespace TalonRAG.Application.Exceptions
{
	/// <summary>
	/// Custom exception class thrown when a user could not be found.
	/// </summary>
	/// <param name="userId">
	/// The unique identifier of the user which could not be found.
	/// </param>
	public class UserNotFoundApiException(int userId) : Exception($"User {userId} not found.")
	{ }
}
