namespace TalonRAG.Domain.SemanticKernel.Embedding
{
	/// <summary>
	/// Interfaces for classes seeking to implement embedding generation services via <see cref="Microsoft.SemanticKernel"/>.
	/// </summary>
	public interface IEmbeddingGenerator
	{
		/// <summary>
		/// Async method to generate embeddings via a list of strings.
		/// TODO: Implement batch embedding of string list. As of version 1.27.0-preview for 
		/// <see cref="Microsoft.SemanticKernel.Connectors.HuggingFace"/>, multiple embedding results per data item are not supported:
		/// "Currently this interface does not support multiple embeddings results per data item, use only one data item"
		/// </summary>
		/// <param name="text">
		/// String list of text to embed.
		/// </param>
		Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> text);
	}
}
