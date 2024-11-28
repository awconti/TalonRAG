using TalonRAG.Domain.Enums;

namespace TalonRAG.Domain.Models
{
	/// <summary>
	/// Model class which represents a single message in a <see cref="Conversation" /> instance on behalf of the domain.
	/// </summary>
	public class Message
	{
		/// <summary>
		/// The role of the author of this message.
		/// </summary>
		public AuthorRole AuthorRole { get; set; } = AuthorRole.System;

		/// <summary>
		/// The string content of the message.
		/// </summary>
		public required string Content { get; set; }
	}
}
