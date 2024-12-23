using TalonRAG.Domain.Enums;

namespace TalonRAG.Domain.Models
{
	/// <summary>
	/// Model representing a message within a conversation on behalf of the domain.
	/// </summary>
	public class Message
	{
		/// <summary>
		/// The unique database identifier for the message.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The <see cref="MessageAuthorRole" /> of the message.
		/// </summary>
		public MessageAuthorRole AuthorRole { get; set; }

		/// <summary>
		/// The content of the message.
		/// </summary>
		public string Content { get; set; } = string.Empty;

		/// <summary>
		/// The date at which the message was created.
		/// </summary>
		public DateTime CreateDate { get; set; }
	}
}
