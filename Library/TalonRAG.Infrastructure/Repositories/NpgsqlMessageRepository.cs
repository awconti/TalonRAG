﻿using Microsoft.Extensions.Options;
using TalonRAG.Domain.Enums;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Domain.Models;
using TalonRAG.Infrastructure.ConfigurationSettings;
using TalonRAG.Infrastructure.Entities;
using TalonRAG.Infrastructure.Extensions;

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
		/// <inheritdoc cref="IMessageRepository.InsertMessageAsync(MessageModel)" />
		public async Task<int> InsertMessageAsync(MessageModel messageModel)
		{
			var sql =
				"INSERT INTO messages (conversation_id, message_type, message_content) VALUES (@ConversationId, @MessageType, @MessageContent) RETURNING id;";

			var parameters = new Dictionary<string, object>
			{
				{ "@ConversationId", messageModel.ConversationId},
				{ "@MessageType", (int) messageModel.MessageType },
				{ "@MessageContent", messageModel.Content }
			};

			return await ExecuteScalarAsync<int>(sql, parameters);
		}

		/// <inheritdoc cref="IMessageRepository.GetMessagesByConversationIdAsync(int)" />
		public async Task<IList<MessageModel>> GetMessagesByConversationIdAsync(int conversationId)
		{
			string sql = $@"
                SELECT id, conversation_id, message_type, message_content, create_date
				FROM messages
				WHERE conversation_id = @ConversationId;
			";

			var parameters = new Dictionary<string, object>
			{
				{ "@ConversationId", conversationId }
			};

			var messageEntities = await ExecuteReaderAsync(
				sql,
				reader => new MessageEntity
				{
					Id = reader.GetInt32(reader.GetOrdinal("id")),
					ConversationId = reader.GetInt32(reader.GetOrdinal("conversation_id")),
					MessageType = (MessageType) reader.GetInt32(reader.GetOrdinal("message_type")),
					Content = reader.GetString(reader.GetOrdinal("message_content")),
					CreateDate = reader.GetDateTime(reader.GetOrdinal("create_date"))
				},
				parameters);

			return messageEntities.Select(entity => entity.ToDomainModel()).ToList();
		}
	}
}
