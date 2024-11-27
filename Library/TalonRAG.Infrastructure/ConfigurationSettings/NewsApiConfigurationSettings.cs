namespace TalonRAG.Infrastructure.ConfigurationSettings
{
	/// <summary>
	/// Configuration settings used exclusive when making HTTP requests to <a href="https://newsapi.org">News API</a>.
	/// </summary>
	public class NewsApiConfigurationSettings
	{
		/// <summary>
		/// The base URL to utilize when making requests to <a href="https://newsapi.org">News API</a>.
		/// </summary>
		public string? BaseUrl { get; set; }

		/// <summary>
		/// The API key to use when requesting data from <a href="https://newsapi.org">News API</a>.
		/// </summary>
		public string? ApiKey { get; set; }
	}
}
