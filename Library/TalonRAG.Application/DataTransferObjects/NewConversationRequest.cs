namespace TalonRAG.Application.DataTransferObjects
{
	/// <summary>
	/// DTO/Request object for initiating a new conversation.
	/// </summary>
	public class NewConversationRequest
	{
		/// <summary>
		/// The unique database identifier of the user.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// The content of the user's initial message.
		/// </summary>
		public string? Message { get; set; }
	}
}
