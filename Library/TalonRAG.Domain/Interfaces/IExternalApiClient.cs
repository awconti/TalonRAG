namespace TalonRAG.Domain.Interfaces
{
	/// <summary>
	/// Interface for classes leveraging external APIs.
	/// </summary>
	public interface IExternalApiClient
	{
		/// <summary>
		/// Retrieves data from an external API.
		/// </summary>
		/// <param name="endpoint">
		/// String representation of the endpoint to use when performing HTTP requests.
		/// </param>
		/// <param name="retryCount">
		/// Optional integer representing the number of times to retry request on failure.
		/// </param>
		Task<T> GetAsync<T>(string endpoint, int retryCount = 5) where T : class, new();
	}
}
