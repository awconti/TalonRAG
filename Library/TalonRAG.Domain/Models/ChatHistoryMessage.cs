using TalonRAG.Domain.Enums;

namespace TalonRAG.Domain.Models
{
	/// <summary>
	/// Represents a single message in a <see cref="ChatHistory" />.
	/// </summary>
	public class ChatHistoryMessage
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
