using TalonRAG.Application.DataTransferObjects;

namespace TalonRAG.Application.Interfaces
{
	/// <summary>
	/// Interface for classes seeking to implement user functionality via API.
	/// </summary>
	public interface IUserApiService
	{
		/// <summary>
		/// Retrieves conversations by user ID.
		/// </summary>
		/// <param name="userId">
		/// The unique identifier of the user to retrieve conversations for.
		/// </param>
		Task<IList<ConversationDto>> GetConversationsByUserIdAsync(int userId);
	}
}
