using TalonRAG.Domain.Models;
using TalonRAG.Infrastructure.Entities;

namespace TalonRAG.Infrastructure.Extensions
{
	/// <summary>
	/// Extension class responsible for constructing instance of <see cref="ArticleEmbeddingModel" /> on behalf of the domain.
	/// </summary>
	public static class ArticleEmbeddingEntityExtensions
	{
		/// <summary>
		/// Creates a new <see cref="ArticleEmbeddingModel" /> based on a <see cref="ArticleEmbeddingEntity" /> instance.
		/// </summary>
		/// <param name="embeddingEntity">
		/// <see cref="ArticleEmbeddingEntity" />.
		/// </param>
		public static ArticleEmbeddingModel ToDomainModel(this ArticleEmbeddingEntity embeddingEntity)
		{
			return new ArticleEmbeddingModel
			{
				Id = embeddingEntity.Id,
				VectorEmbedding = embeddingEntity.VectorEmbedding,
				Content = embeddingEntity.Content
			};
		}
	}
}
