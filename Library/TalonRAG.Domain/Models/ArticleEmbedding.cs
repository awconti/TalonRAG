using TalonRAG.Domain.Entities;

namespace TalonRAG.Domain.Models
{
	/// <summary>
	/// Model class representing an instance of <see cref="Entities.ArticleEmbeddingRecord" /> on behalf of the domain.
	/// </summary>
	public class ArticleEmbedding
	{
		public required ArticleEmbeddingRecord ArticleEmbeddingRecord { get; set; }
	}
}
