using TalonRAG.Domain.Entities;

namespace TalonRAG.Domain.Interfaces
{
    /// <summary>
    /// Interface for repository classes seeking to implement vector embedding database operations for <see cref="EmbeddingRecord" /> instances on behalf of the domain.
    /// </summary>
    public interface IEmbeddingRepository
	{
		/// <summary>
		/// Purges all vector embeddings.
		/// </summary>
		/// <param name="createDate">
		/// Optional maximum create date to consider when purging vector embeddings.
		/// </param>
		Task DeleteAllEmbeddingsAsync(DateTime? createDate = null);

		/// <summary>
		/// Inserts a list collection of <see cref="EmbeddingRecord" />.
		/// TODO: Replace w/ batched bulk insert.
		/// </summary>
		/// <param name="embeddingRecords">
		/// List collection of <see cref="EmbeddingRecord" />.
		/// </param>
		Task InsertEmbeddingsAsync(IList<EmbeddingRecord> embeddingRecords);

		/// <summary>
		/// Bulk inserts a list collection of <see cref="EmbeddingRecord" />.
		/// </summary>
		/// <param name="embeddingRecords">
		/// List collection of <see cref="EmbeddingRecord" />.
		/// </param>
		Task BulkInsertEmbeddingsAsync(IList<EmbeddingRecord> embeddingRecords);

		/// <summary>
		/// Retrieves similar embeddings.
		/// </summary>
		/// <param name="embedding">
		/// Float array used as as basis to retrieve similar embeddings.
		/// </param>
		/// <param name="limit">
		/// Number of embeddings to retrieve.
		/// </param>
		Task<IList<EmbeddingRecord>> GetSimilarEmbeddingsAsync(float[] embedding, int limit = 5);
	}
}
