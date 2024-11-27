using TalonRAG.Domain.Entities;

namespace TalonRAG.Domain.Interfaces
{
    /// <summary>
    /// Interface for repository classes seeking to implement vector embedding database operations for <see cref="ArticleEmbedding" /> instances on behalf of the domain.
    /// </summary>
    public interface IArticleEmbeddingRepository
	{
		/// <summary>
		/// Purges all vector embeddings.
		/// </summary>
		Task DeleteAllEmbeddingsAsync();

		/// <summary>
		/// Inserts a list collection of <see cref="ArticleEmbedding"/>.
		/// TODO: Replace w/ batched bulk insert.
		/// </summary>
		/// <param name="embeddings">
		/// List collection of <see cref="ArticleEmbedding"/>.
		/// </param>
		Task InsertEmbeddingsAsync(IList<ArticleEmbedding> embeddings);

		/// <summary>
		/// Bulk inserts a list collection of <see cref="ArticleEmbedding"/>.
		/// </summary>
		/// <param name="embeddings">
		/// List collection of <see cref="ArticleEmbedding"/>.
		/// </param>
		Task BulkInsertEmbeddingsAsync(IList<ArticleEmbedding> embeddings);

		/// <summary>
		/// Retrieves similar embeddings.
		/// </summary>
		/// <param name="embedding">
		/// Float array used as as basis to retrieve similar embeddings.
		/// </param>
		/// <param name="limit">
		/// Number of embeddings to retrieve.
		/// </param>
		Task<IList<ArticleEmbedding>> GetSimilarEmbeddingsAsync(float[] embedding, int limit = 5);
	}
}
