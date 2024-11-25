namespace TalonRAG.Domain.Interfaces
{
	/// <summary>
	/// Interface for classes seeking to implement embedding generation services on behalf of the domain.
	/// </summary>
	public interface IEmbeddingGenerator
	{
		/// <summary>
		/// Async method to generate embeddings via a list of strings.
		/// </summary>
		/// <param name="text">
		/// String list of text to embed.
		/// </param>
		Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> text);
	}
}
