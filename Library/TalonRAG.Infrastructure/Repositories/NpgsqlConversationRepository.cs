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
		/// <inheritdoc cref="IConversationRepository.InsertConversationsAsync(IList{ConversationRecord})" />
		public async Task InsertConversationsAsync(IList<ConversationRecord> conversations)
		{
			foreach (var conversation in conversations)
			{
				var sql =
					"INSERT INTO conversations (user_id) VALUES (@UserId);";

				var parameters = new Dictionary<string, object>
				{
					{ "@UserId", conversation.UserId }
				};

				await ExecuteNonQueryAsync(sql, parameters);
			}
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
