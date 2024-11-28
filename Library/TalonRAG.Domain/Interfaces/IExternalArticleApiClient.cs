namespace TalonRAG.Domain.Interfaces
{
	/// <summary>
	/// Interface for classes leveraging external APIs to retrieve news articles on behalf of the domain.
	/// </summary>
	public interface IExternalArticleApiClient
	{
		/// <summary>
		/// Retrieves news articles from an external API.
		/// </summary>
		/// <param name="endpoint">
		/// String representation of the endpoint to use when performing HTTP requests.
		/// </param>
		Task<T> GetArticlesAsync<T>(string endpoint) where T : class, new();
	}
}
