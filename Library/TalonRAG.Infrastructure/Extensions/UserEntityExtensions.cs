using TalonRAG.Domain.Models;
using TalonRAG.Infrastructure.Entities;

namespace TalonRAG.Infrastructure.Extensions
{
	/// <summary>
	/// Mapper class responsible for constructing instances of <see cref="UserModel" /> on behalf of the domain.
	/// </summary>
	public static class UserEntityExtensions
	{
		/// <summary>
		/// Creates a new <see cref="UserModel " /> based on a <see cref="UserEntity" /> instance.
		/// </summary>
		/// <param name="userEntity">
		/// <see cref="userEntity" />.
		/// </param>
		public static UserModel ToDomainModel(this UserEntity userEntity)
		{
			return new UserModel
			{
				Id = userEntity.Id,
				Email = userEntity.Email
			};
		}
	}
}
