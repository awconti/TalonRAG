using TalonRAG.Domain.Entities;
using TalonRAG.Domain.Models;

namespace TalonRAG.Domain.Extensions
{
	/// <summary>
	/// Mapper class responsible for constructing instance of <see cref="Embedding" /> on behalf of the domain.
	/// </summary>
	public static class EmbeddingRecordExtensions
	{
		/// <summary>
		/// Creates a new <see cref="Embedding" /> based on a <see cref="EmbeddingRecord" /> instance.
		/// </summary>
		/// <param name="embeddingRecord">
		/// <see cref="EmbeddingRecord" />.
		/// </param>
		public static Embedding ToDomainModel(this EmbeddingRecord embeddingRecord)
		{
			return new Embedding
			{
				Id = embeddingRecord.Id,
				VectorEmbedding = embeddingRecord.VectorEmbedding,
				Content = embeddingRecord.Content
			};
		}
	}
}
