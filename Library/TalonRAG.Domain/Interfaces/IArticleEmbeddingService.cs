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
		/// <param name="articleDescriptions">
		/// List collection of strings, representing the descriptions of articles.
		/// </param>
		/// <param name="maxArticleDate">
		/// The maximum date time to consider when replacing stored article descriptions as embeddings.
		/// </param>
		Task CreateEmbeddingsForArticleDescriptionsAsync(IList<string> articleDescriptions, DateTime maxArticleDate);

		/// <summary>
		/// Retrieves a list of similar article embeddings based on message content.
		/// </summary>
		/// <param name="messageContent">
		/// The content of the message.
		/// </param>
		Task<IList<ArticleEmbedding>> GetSimilarArticleEmbeddingsForMessageContentAsync(string messageContent);
	}
}
