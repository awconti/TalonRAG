using Microsoft.Extensions.Options;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Domain.Models;
using TalonRAG.Infrastructure.ConfigurationSettings;
using TalonRAG.Infrastructure.Entities;
using TalonRAG.Infrastructure.Extensions;

namespace TalonRAG.Infrastructure.Repositories
{
	/// <summary>
	/// Npgsql repository implementation of <see cref="IUserRepository"/>.
	/// </summary>
	/// <param name="options">
	/// <see cref="IOptions{DatabaseConfigurationSettings}" />.
	/// </param>
	public class NpgsqlUserRepository(IOptions<DatabaseConfigurationSettings> options) : BaseNpgsqlRepository(options), IUserRepository
	{
		/// <inheritdoc cref="IUserRepository.GetUserByIdAsync(int)" />
		public async Task<UserModel?> GetUserByIdAsync(int userId)
		{
			string sql = $@"
                SELECT id, email, create_date
				FROM users
				WHERE id = @UserId
			";

			var parameters = new Dictionary<string, object>
			{
				{ "@UserId", userId }
			};

			var userEntities = await ExecuteReaderAsync(
				sql,
				reader => new UserEntity
				{
					Id = reader.GetInt32(reader.GetOrdinal("id")),
					Email = reader.GetString(reader.GetOrdinal("email")),
					CreateDate = reader.GetDateTime(reader.GetOrdinal("create_date"))
				},
				parameters);

			return userEntities.FirstOrDefault()?.ToDomainModel();
		}
	}
}
