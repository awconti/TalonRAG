using TalonRAG.Domain.Entities;
using TalonRAG.Domain.Models;

namespace TalonRAG.Domain.Extensions
{
	/// <summary>
	/// Mapper class responsible for constructing instances of <see cref="Message" /> on behalf of the domain.
	/// </summary>
	public static class MessageRecordExtensions
	{
		/// <summary>
		/// Creates a new <see cref="Message " /> based on a <see cref="MessageRecord" /> instance.
		/// </summary>
		/// <param name="messageRecord">
		/// <see cref="MessageRecord" />.
		/// </param>
		public static Message ToDomainModel(this MessageRecord messageRecord)
		{
			return new Message
			{
				Id = messageRecord.Id,
				AuthorRole = messageRecord.MessageAuthorRole,
				Content = messageRecord.Content,
				CreateDate = messageRecord.CreateDate
			};
		}
	}
}
