using Microsoft.SemanticKernel.ChatCompletion;

namespace TalonRAG.Infrastructure.SemanticKernel.ChatCompletion
{
    /// <summary>
    /// <see cref="ChatHistory"/> specific instance for the TalonRAG solution.
    /// </summary>
    public class TalonRAGChatHistory(string systemMessage) : ChatHistory(systemMessage)
    {
        /// <summary>
        /// Adds a tool message to the chat history.
        /// </summary>
        /// <param name="message">
        /// Message content.
        /// </param>
        public void AddToolMessage(string message)
        {
            AddMessage(AuthorRole.Tool, message);
        }
    }
}
