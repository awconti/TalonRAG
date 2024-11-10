namespace TalonRAG.Common.Embedding
{
	/// <summary>
	/// Interfaces for classes seeking to implement embedding generation services via <see cref="Microsoft.SemanticKernel"/>.
	/// </summary>
	public interface IEmbeddingGenerator
	{
		/// <summary>
		/// Async method to generate embeddings via a list of strings.
		/// TODO: Implement batch embedding of string list.
		/// </summary>
		/// <param name="text">
		/// String list of text to embed.
		/// </param>
		Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> text);
	}
}
