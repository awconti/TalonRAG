using TalonRAG.Domain.Models;

namespace TalonRAG.Domain.Interfaces
{
	/// <summary>
	/// Interface for classes seeking to implement chat completion services on behalf of the domain.
	/// </summary>
	public interface IChatCompletionService
	{
		/// <summary>
		/// Async method to retrieve content from a request to a specific language model.
		/// </summary>
		/// <param name="chatHistory">
		/// Object containing history of prior input from user and responses from the language model.
		/// </param>
		Task<string> GetChatMessageContentAsync(ConversationModel chatHistory);
	}
}
