using TalonRAG.Application.DataTransferObjects.External;
using TalonRAG.Application.Interfaces;
using TalonRAG.Domain.Interfaces;

namespace TalonRAG.Application.Services
{
    /// <summary>
    /// ETL console application service class implementation of <see cref="IConsoleAppService" />.
    /// </summary>
    /// <param name="embeddingService">
    /// <see cref="IArticleEmbeddingService" />.
    /// </param>
	/// <param name="newsApiClient">
	/// <see cref="IExternalArticleApiClient" />.
	/// </param>
    public class EtlConsoleAppService(
		IArticleEmbeddingService embeddingService, IExternalArticleApiClient newsApiClient) : IConsoleAppService
	{
		private readonly IArticleEmbeddingService _embeddingService = embeddingService;
		private readonly IExternalArticleApiClient _newsApiClient = newsApiClient;

		/// <inheritdoc cref="IConsoleAppService.RunAsync" />
		public async Task RunAsync()
		{
			try
			{
				var maxArticleDate = DateTime.UtcNow.AddDays(-30);

				var articles = await GetNewsApiArticlesAsync(maxArticleDate);
				var articleDescriptions = 
					articles.Select(article => article.Description)
						.Where(description => description != null)
						.ToList();

				await _embeddingService.CreateEmbeddingsForContentAsync(articleDescriptions, maxArticleDate);
			} 
			catch (Exception ex)
			{
				Console.WriteLine($"Encountered an exception - {ex.Message}");
			}
		}

		private async Task<IList<NewsApiV2ArticleDto>> GetNewsApiArticlesAsync(DateTime maxArticleDate)
		{
			var response = 
				await _newsApiClient.GetArticlesAsync<NewsApiV2Response>($"everything?qInTitle=philadelphia eagles&sortBy=publishedAt&from={maxArticleDate:yyyy-MM-dd}");

			return response.Articles ?? [];
		}
	}
}