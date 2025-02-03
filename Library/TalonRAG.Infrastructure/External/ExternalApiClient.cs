using Polly;
using Polly.Extensions.Http;
using System.Text.Json;
using TalonRAG.Domain.Interfaces;

namespace TalonRAG.Infrastructure.External
{
	/// <summary>
	/// Specific implementation of <see cref="IExternalApiClient"/> utilized to perform HTTP requests to APIs externally.
	/// </summary>
	/// <param name="httpClient">
	/// <see cref="HttpClient" />.
	/// </param>
	public class ExternalApiClient(HttpClient httpClient) : IExternalApiClient
	{
		private readonly HttpClient _httpClient = httpClient;

		/// <inheritdoc cref="IExternalApiClient.GetAsync(string)" />
		public async Task<T> GetAsync<T>(string endpoint, int retryCount = 5) where T : class, new()
		{
			var retryPolicy = HttpPolicyExtensions
				.HandleTransientHttpError()
				.OrResult(r => !r.IsSuccessStatusCode)
				.WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

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
