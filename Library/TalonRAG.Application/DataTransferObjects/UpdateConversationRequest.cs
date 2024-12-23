namespace TalonRAG.Application.DataTransferObjects
{
	/// <summary>
	/// DTO/Request object for adding new messages to a conversation.
	/// </summary>
	public class UpdateConversationRequest
	{
		/// <summary>
		/// The string content of the user's message to add.
		/// </summary>
		public string? Message { get; set; }
	}
}
