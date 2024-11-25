using TalonRAG.Domain.Models;

namespace TalonRAG.Domain.Enums
{
	/// <summary>
	/// The role of the author of a <see cref="ChatHistoryMessage" />.
	/// </summary>
	public enum AuthorRole
	{
		/// <summary>
		/// The role that defines messages initiated by the AI assistant.
		/// </summary>
		Assistant = 0,

		/// <summary>
		/// The role that defines messages initiated by the user.
		/// </summary>
		User = 1,

		/// <summary>
		/// The role that defines messages provided to the AI assistant as information.
		/// </summary>
		Tool = 2,

		/// <summary>
		/// The role that defines messages pertaining to the behavior of the AI assitant.
		/// </summary>
		System = 3
	}
}
