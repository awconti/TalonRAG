using TalonRAG.Domain.Models;

namespace TalonRAG.Domain.Interfaces
{
	/// <summary>
	/// Interface for repository classes seeking to implement database operations for <see cref="UserModel" /> instances on behalf of the domain.
	/// </summary>
	public interface IUserRepository
	{
		/// <summary>
		/// Retrieves a user by ID.
		/// </summary>
		/// <param name="userId">
		/// The unique identifier of the user.
		/// </param>
		Task<UserModel?> GetUserByIdAsync(int userId);
	}
}
