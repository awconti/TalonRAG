using Polly;
using Polly.Extensions.Http;
using System.Text.Json;
using TalonRAG.Domain.Interfaces;

namespace TalonRAG.Infrastructure.NewsAPI
{
	/// <summary>
	/// <a href="https://newsapi.org">News API</a> specific implementation of <see cref="IExternalArticleApiClient"/>.
	/// </summary>
	/// <param name="httpClient">
	/// <see cref="HttpClient" />.
	/// </param>
	public class NewsApiClient(HttpClient httpClient) : IExternalArticleApiClient
	{
		private readonly HttpClient _httpClient = httpClient;

		/// <inheritdoc cref="IExternalArticleApiClient.GetArticlesAsync(string)" />
		public async Task<T> GetArticlesAsync<T>(string endpoint) where T : class, new()
		{
			var retryPolicy = HttpPolicyExtensions
				.HandleTransientHttpError()
				.OrResult(r => !r.IsSuccessStatusCode)
				.WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

			var response = await retryPolicy.ExecuteAsync(async () =>
			{
				return await _httpClient.GetAsync(endpoint);
			});
			response.EnsureSuccessStatusCode();

			var jsonResponse = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<T>(jsonResponse) ?? new();
		}
	}
}
