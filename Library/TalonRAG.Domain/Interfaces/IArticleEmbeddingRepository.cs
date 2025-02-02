using TalonRAG.Domain.Models;

namespace TalonRAG.Domain.Interfaces
{
    /// <summary>
    /// Interface for repository classes seeking to implement vector embedding database operations for <see cref="ArticleEmbeddingModel" /> instances on behalf of the domain.
    /// </summary>
    public interface IArticleEmbeddingRepository
	{
		/// <summary>
		/// Retrieves similar embeddings.
		/// </summary>
		/// <param name="embedding">
		/// Float array used as as basis to retrieve similar embeddings.
		/// </param>
		/// <param name="limit">
		/// Number of embeddings to retrieve.
		/// </param>
		Task<IList<ArticleEmbeddingModel>> GetSimilarEmbeddingsAsync(float[] embedding, int limit = 5);

		/// <summary>
		/// Purges all vector embeddings.
		/// </summary>
		/// <param name="createDate">
		/// Optional maximum create date to consider when purging vector embeddings.
		/// </param>
		Task DeleteAllEmbeddingsAsync(DateTime? createDate = null);

		/// <summary>
		/// Inserts a list collection of <see cref="ArticleEmbeddingModel" />.
		/// TODO: Replace w/ batched bulk insert.
		/// </summary>
		/// <param name="embeddingModels">
		/// List collection of <see cref="ArticleEmbeddingModel" />.
		/// </param>
		Task InsertEmbeddingsAsync(IList<ArticleEmbeddingModel> embeddingModels);

		/// <summary>
		/// Bulk inserts a list collection of <see cref="ArticleEmbeddingModel" />.
		/// </summary>
		/// <param name="embeddingModels">
		/// List collection of <see cref="ArticleEmbeddingModel" />.
		/// </param>
		Task BulkInsertEmbeddingsAsync(IList<ArticleEmbeddingModel> embeddingModels);
	}
}
