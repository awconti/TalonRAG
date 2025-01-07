using TalonRAG.Domain.Models;

namespace TalonRAG.Domain.Interfaces
{
	/// <summary>
	/// Interface for classes settings to embed articles on behalf of the domain.
	/// </summary>
	public interface IArticleEmbeddingService
	{
		/// <summary>
		/// Creates embeddings for a list of article descriptions.
		/// </summary>
		/// <param name="content">
		/// String list collection of content to embed.
		/// </param>
		/// <param name="maxDate">
		/// The maximum date time to consider when replacing stored embeddings.
		/// </param>
		Task CreateEmbeddingsForContentAsync(IList<string> content, DateTime maxDate);

		/// <summary>
		/// Retrieves a list of similar  embeddings based on provided string content.
		/// </summary>
		/// <param name="content">
		/// The content to use when retrieving similar embeddings.
		/// </param>
		Task<IList<ArticleEmbeddingModel>> GetSimilarEmbeddingsFromContentAsync(string content);
	}
}
