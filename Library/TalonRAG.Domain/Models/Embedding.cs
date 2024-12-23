namespace TalonRAG.Domain.Models
{
	/// <summary>
	/// Model class representing an embedding on behalf of the domain.
	/// </summary>
	public class Embedding
	{
		/// <summary>
		/// The unique database identifier for the article embedding.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Embedding representing description of article.
		/// </summary>
		public float[]? VectorEmbedding { get; set; }

		/// <summary>
		/// Description of article.
		/// </summary>
		public string? Content { get; set; }
	}
}
