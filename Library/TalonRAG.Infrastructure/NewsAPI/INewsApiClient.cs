using TalonRAG.Infrastructure.DataTransferObjects;

namespace TalonRAG.Infrastructure.NewsAPI
{
	/// <summary>
	/// Interface for client classes seeking to invoke <a href="https://newsapi.org">News API</a> on behalf of the infrastructure.
	/// </summary>
	public interface INewsApiClient
	{
		/// <summary>
		/// Retrieves an instance of <see cref="NewsApiResponse" /> via <a href="https://newsapi.org">News API</a>.
		/// </summary>
		/// <param name="endpoint">
		/// String representation of the endpoint to use when performing HTTP requests.
		/// </param>
		Task<NewsApiResponse> GetAsync(string endpoint);
	}
}
