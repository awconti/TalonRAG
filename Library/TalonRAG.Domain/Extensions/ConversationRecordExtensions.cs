using TalonRAG.Domain.Entities;
using TalonRAG.Domain.Models;

namespace TalonRAG.Domain.Extensions
{
	/// <summary>
	/// Mapper class responsible for constructing instances of <see cref="Conversation" /> on behalf of the domain.
	/// </summary>
	public static class ConversationRecordExtensions
	{
		/// <summary>
		/// Creates a new <see cref="Conversation" /> instance based on a <see cref="ConversationRecord" /> instance 
		/// and an optional collection of <see cref="MessageRecord" /> instances.
		/// </summary>
		/// <param name="conversationRecord">
		/// <see cref="ConversationRecord" />.
		/// </param>
		/// <param name="messageRecords">
		/// An optional collection of <see cref="MessageRecord" /> instances. 
		/// </param>
		public static Conversation ToDomainModel(this ConversationRecord conversationRecord, IList<MessageRecord>? messageRecords = null)
		{
			var conversation = new Conversation
			{
				Id = conversationRecord.Id,
				UserId = conversationRecord.UserId,
				CreateDate = conversationRecord.CreateDate
			};

			if (messageRecords != null)
			{
				var messages = messageRecords.Select(record => record.ToDomainModel()).ToList();
				conversation.SetMessages(messages);
			}

			return conversation;
		}
	}
}
