using Microsoft.Extensions.Options;
using TalonRAG.Domain.Entities;
using TalonRAG.Domain.Enums;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Infrastructure.ConfigurationSettings;

namespace TalonRAG.Infrastructure.Repositories
{
	/// <summary>
	/// Npgsql repository implementation of <see cref="IMessageRepository" />.
	/// </summary>
	/// <param name="options">
	/// <see cref="IOptions{DatabaseConfigurationSettings}" />.
	/// </param>
	public class NpgsqlMessageRepository(IOptions<DatabaseConfigurationSettings> options) : BaseNpgsqlRepository(options), IMessageRepository
	{
		/// <inheritdoc cref="IMessageRepository.InsertMessagesAsync(IList{MessageRecord})" />
		public async Task InsertMessagesAsync(IList<MessageRecord> messages)
		{
			foreach (var message in messages)
			{
				var sql =
					"INSERT INTO messages (conversation_id, author_role, message_content) VALUES (@ConversationId, @AuthorRole, @MessageContent);";

				var parameters = new Dictionary<string, object>
				{
					{ "@ConversationId", message.ConversationId },
					{ "@AuthorRole", message.MessageAuthorRole },
					{ "@MessageContent", message.Content }
				};

				await ExecuteNonQueryAsync(sql, parameters);
			}
		}

		/// <inheritdoc cref="IMessageRepository.GetMessagesByConversationIdAsync(int)" />
		public async Task<IList<MessageRecord>> GetMessagesByConversationIdAsync(int conversationId)
		{
			string sql = $@"
                SELECT id, conversation_id, author_role, message_content, create_date
				FROM messages
				WHERE conversation_id = @ConversationId;
			";

			var parameters = new Dictionary<string, object>
			{
				{ "@ConversationId", conversationId }
			};

			return await ExecuteReaderAsync(
				sql,
				reader => new MessageRecord
				{
					Id = reader.GetInt32(reader.GetOrdinal("id")),
					ConversationId = reader.GetInt32(reader.GetOrdinal("conversation_id")),
					MessageAuthorRole = reader.GetFieldValue<MessageAuthorRole>(reader.GetOrdinal("author_role")),
					Content = reader.GetString(reader.GetOrdinal("message_content")),
					CreateDate = reader.GetDateTime(reader.GetOrdinal("create_date"))
				},
				parameters);
		}
	}
}
