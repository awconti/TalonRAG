using Microsoft.SemanticKernel;

namespace TalonRAG.Common.ChatCompletion
{
	/// <summary>
	/// Interface for classes seeking to implement chat completion services via <see cref="Microsoft.SemanticKernel"/>.
	/// </summary>
	public interface IChatCompletor
	{
		/// <summary>
		/// Async method to retrieve <see cref="ChatMessageContent"/> from a specific language model.
		/// </summary>
		/// <param name="prompt">
		/// String representation of the provided prompt to the language model.
		/// </param>
		Task<ChatMessageContent> GetChatMessageContentAsync(string prompt);
	}
}
