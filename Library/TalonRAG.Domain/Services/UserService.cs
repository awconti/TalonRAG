using TalonRAG.Domain.Interfaces;
using TalonRAG.Domain.Models;

namespace TalonRAG.Domain.Services
{
	/// <summary>
	/// User service class implementation of <see cref="IUserService"/>. 
	/// </summary>
	/// <param name="userRepository">
	/// <see cref="IUserRepository"/>.
	/// </param>
	public class UserService(IUserRepository userRepository) : IUserService
	{
		private readonly IUserRepository _userRepository = userRepository;

		/// <inheritdoc cref="IUserService.GetUserByIdAsync(int)" />
		public async Task<UserModel?> GetUserByIdAsync(int userId) => await _userRepository.GetUserByIdAsync(userId);
	}
}
