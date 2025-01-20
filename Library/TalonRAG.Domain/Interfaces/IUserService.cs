using TalonRAG.Domain.Models;

namespace TalonRAG.Domain.Interfaces
{
	/// <summary>
	/// Interface for classes seeking to manager users in the context of conversations on behalf of the domain.
	/// </summary>
	public interface IUserService
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
