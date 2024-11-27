using System.Text.Json;
using TalonRAG.Infrastructure.DataTransferObjects;

namespace TalonRAG.Infrastructure.NewsAPI
{
	/// <summary>
	/// Generic implementation of <see cref="INewsApiClient" />.
	/// </summary>
	/// <param name="httpClient">
	/// <see cref="HttpClient" />.
	/// </param>
	public class NewsApiClient(HttpClient httpClient) : INewsApiClient
	{
		private readonly HttpClient _httpClient = httpClient;

		/// <inheritdoc cref="INewsApiClient.GetAsync(string)" />
		public async Task<NewsApiResponse> GetAsync(string endpoint)
		{
			var response = await _httpClient.GetAsync(endpoint);
			response.EnsureSuccessStatusCode();

			var jsonResponse = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<NewsApiResponse>(jsonResponse) ?? new();
		}
	}
}
