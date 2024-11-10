namespace TalonRAG.Common.Persistence.DTO
{
	/// <summary>
	/// Represents embedded content for instances of <see cref="Article"/>.
	/// </summary>
	public class ArticleEmbedding : Microsoft.Extensions.AI.Embedding
	{
		/// <summary>
		/// Float array representing <see cref="Article.Description"/>.
		/// </summary>
		public float[]? Embedding { get; set; }

		/// <summary>
		/// <see cref="Article.Description"/>
		/// </summary>
		public string? Content { get; set; }
	}
}
