using Microsoft.Extensions.Options;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Domain.Models;
using TalonRAG.Infrastructure.ConfigurationSettings;
using TalonRAG.Infrastructure.Entities;
using TalonRAG.Infrastructure.Extensions;

namespace TalonRAG.Infrastructure.Repositories
{
	/// <summary>
	/// Npgsql repository implementation of <see cref="IConversationRepository" />.
	/// </summary>
	/// <param name="options">
	/// <see cref="IOptions{DatabaseConfigurationSettings}" />.
	/// </param>
	public class NpgsqlConversationRepository(IOptions<DatabaseConfigurationSettings> options) : BaseNpgsqlRepository(options), IConversationRepository
	{
		/// <inheritdoc cref="IConversationRepository.InsertConversationAsync(int)" />
		public async Task<int> InsertConversationAsync(int userId)
		{
			var sql =
				"INSERT INTO conversations (user_id) VALUES (@UserId) RETURNING id;";

			var parameters = new Dictionary<string, object>
			{
				{ "@UserId", userId }
			};

			return await ExecuteScalarAsync<int>(sql, parameters);
		}

		/// <inheritdoc cref="IConversationRepository.GetConversationByIdAsync(int)" />
		public async Task<ConversationModel?> GetConversationByIdAsync(int conversationId)
		{
			string sql = $@"
                SELECT id, user_id, create_date
				FROM conversations
				WHERE id = @ConversationId
			";

			var parameters = new Dictionary<string, object>
			{
				{ "@ConversationId", conversationId }
			};

			var conversationEntities = await ExecuteReaderAsync(
				sql,
				reader => new ConversationEntity
				{
					Id = reader.GetInt32(reader.GetOrdinal("id")),
					UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
					CreateDate = reader.GetDateTime(reader.GetOrdinal("create_date"))
				},
				parameters);

			return conversationEntities.FirstOrDefault().ToDomainModel();
		}

		/// <inheritdoc cref="IConversationRepository.GetConversationsByUserIdAsync(int)" />
		public async Task<IList<ConversationModel>> GetConversationsByUserIdAsync(int userId)
		{
			string sql = $@"
                SELECT id, user_id, create_date
				FROM conversations
				WHERE user_id = @UserId
			";

			var parameters = new Dictionary<string, object>
			{
				{ "@UserId", userId }
			};

			var conversationEntities = await ExecuteReaderAsync(
				sql,
				reader => new ConversationEntity
				{
					Id = reader.GetInt32(reader.GetOrdinal("id")),
					UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
					CreateDate = reader.GetDateTime(reader.GetOrdinal("create_date"))
				},
				parameters);

			return conversationEntities.Select(entity => entity.ToDomainModel()).ToList();
		}
	}
}
