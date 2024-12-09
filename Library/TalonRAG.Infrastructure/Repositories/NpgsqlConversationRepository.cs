using Microsoft.Extensions.Options;
using TalonRAG.Domain.Entities;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Infrastructure.ConfigurationSettings;

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
		/// <inheritdoc cref="IConversationRepository.InsertConversationAsync(ConversationRecord)" />
		public async Task<int> InsertConversationAsync(ConversationRecord conversationRecord)
		{
			var sql =
				"INSERT INTO conversations (user_id) VALUES (@UserId) RETURNING id;";

			var parameters = new Dictionary<string, object>
			{
				{ "@UserId", conversationRecord.UserId }
			};

			return await ExecuteScalarAsync<int>(sql, parameters);
		}

		/// <inheritdoc cref="IConversationRepository.GetConversationByIdAsync(int)" />
		public async Task<ConversationRecord?> GetConversationByIdAsync(int conversationId)
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

			var conversationRecords = await ExecuteReaderAsync(
				sql,
				reader => new ConversationRecord
				{
					Id = reader.GetInt32(reader.GetOrdinal("id")),
					UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
					CreateDate = reader.GetDateTime(reader.GetOrdinal("create_date"))
				},
				parameters);

			return conversationRecords.FirstOrDefault();
		}

		/// <inheritdoc cref="IConversationRepository.GetConversationsByUserIdAsync(int)" />
		public async Task<IList<ConversationRecord>> GetConversationsByUserIdAsync(int userId)
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

			return await ExecuteReaderAsync(
				sql,
				reader => new ConversationRecord
				{
					Id = reader.GetInt32(reader.GetOrdinal("id")),
					UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
					CreateDate = reader.GetDateTime(reader.GetOrdinal("create_date"))
				},
				parameters);
		}
	}
}
