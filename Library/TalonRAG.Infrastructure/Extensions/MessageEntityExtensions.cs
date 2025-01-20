using TalonRAG.Domain.Models;
using TalonRAG.Infrastructure.Entities;

namespace TalonRAG.Infrastructure.Extensions
{
	/// <summary>
	/// Mapper class responsible for constructing instances of <see cref="MessageModel" /> on behalf of the domain.
	/// </summary>
	public static class MessageEntityExtensions
	{
		/// <summary>
		/// Creates a new <see cref="MessageModel " /> based on a <see cref="MessageEntity" /> instance.
		/// </summary>
		/// <param name="messageEntity">
		/// <see cref="MessageEntity" />.
		/// </param>
		public static MessageModel ToDomainModel(this MessageEntity messageEntity)
		{
			return new MessageModel
			{
				Id = messageEntity.Id,
				ConversationId = messageEntity.ConversationId,
				MessageType = messageEntity.MessageType,
				Content = messageEntity.Content,
				CreateDate = messageEntity.CreateDate
			};
		}
	}
}
