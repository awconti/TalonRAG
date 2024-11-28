using TalonRAG.Domain.Entities;

namespace TalonRAG.Domain.Interfaces
{
    /// <summary>
    /// Interface for repository classes seeking to implement vector embedding database operations for <see cref="ArticleEmbeddingRecord" /> instances on behalf of the domain.
    /// </summary>
    public interface IArticleEmbeddingRepository
	{
		/// <summary>
		/// Purges all vector embeddings.
		/// </summary>
		/// <param name="createDate">
		/// Optional maximum create date to consider when purging vector embeddings.
		/// </param>
		Task DeleteAllEmbeddingsAsync(DateTime? createDate = null);

		/// <summary>
		/// Inserts a list collection of <see cref="ArticleEmbeddingRecord"/>.
		/// TODO: Replace w/ batched bulk insert.
		/// </summary>
		/// <param name="embeddings">
		/// List collection of <see cref="ArticleEmbeddingRecord"/>.
		/// </param>
		Task InsertEmbeddingsAsync(IList<ArticleEmbeddingRecord> embeddings);

		/// <summary>
		/// Bulk inserts a list collection of <see cref="ArticleEmbeddingRecord"/>.
		/// </summary>
		/// <param name="embeddings">
		/// List collection of <see cref="ArticleEmbeddingRecord"/>.
		/// </param>
		Task BulkInsertEmbeddingsAsync(IList<ArticleEmbeddingRecord> embeddings);

		/// <summary>
		/// Retrieves similar embeddings.
		/// </summary>
		/// <param name="embedding">
		/// Float array used as as basis to retrieve similar embeddings.
		/// </param>
		/// <param name="limit">
		/// Number of embeddings to retrieve.
		/// </param>
		Task<IList<ArticleEmbeddingRecord>> GetSimilarEmbeddingsAsync(float[] embedding, int limit = 5);
	}
}
