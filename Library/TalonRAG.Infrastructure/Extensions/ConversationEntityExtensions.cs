using TalonRAG.Domain.Models;
using TalonRAG.Infrastructure.Entities;

namespace TalonRAG.Infrastructure.Extensions
{
	/// <summary>
	/// Extension class responsible for constructing instances of <see cref="ConversationModel" /> on behalf of the domain.
	/// </summary>
	public static class ConversationEntityExtensions
	{
		/// <summary>
		/// Creates a new <see cref="ConversationModel" /> instance based on a <see cref="ConversationEntity" /> instance 
		/// and an optional collection of <see cref="MessageEntity" /> instances.
		/// </summary>
		/// <param name="conversationEntity">
		/// <see cref="ConversationEntity" />.
		/// </param>
		/// <param name="messageEntities">
		/// An optional collection of <see cref="MessageEntity" /> instances. 
		/// </param>
		public static ConversationModel ToDomainModel(this ConversationEntity conversationEntity, IList<MessageEntity>? messageEntities = null)
		{
			var conversation= new ConversationModel
			{
				Id = conversationEntity.Id,
				UserId = conversationEntity.UserId,
				CreateDate = conversationEntity.CreateDate
			};

			if (messageEntities != null)
			{
				var messages = messageEntities.Select(entity => entity.ToDomainModel()).ToList();
				conversation.SetMessages(messages);
			}

			return conversation;
		}
	}
}
