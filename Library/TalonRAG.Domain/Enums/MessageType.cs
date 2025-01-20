using System.ComponentModel;
using TalonRAG.Domain.Models;

namespace TalonRAG.Domain.Enums
{
	/// <summary>
	/// The role of the author of a <see cref="MessageModel" />.
	/// </summary>
	public enum MessageType
	{
		/// <summary>
		/// The role that defines messages initiated by the AI assistant.
		/// </summary>
		[Description("Assistant")]
		Assistant = 0,

		/// <summary>
		/// The role that defines messages initiated by the user.
		/// </summary>
		[Description("User")]
		User = 1,

		/// <summary>
		/// The role that defines messages provided to the AI assistant as information.
		/// </summary>
		[Description("Tool")]
		Tool = 2,

		/// <summary>
		/// The role that defines messages pertaining to the behavior of the AI assitant.
		/// </summary>
		[Description("System")]
		System = 3
	}
}
