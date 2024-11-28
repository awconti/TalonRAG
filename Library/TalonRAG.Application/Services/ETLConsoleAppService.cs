using TalonRAG.Application.DataTransferObjects;
using TalonRAG.Domain.Entities;
using TalonRAG.Domain.Interfaces;

namespace TalonRAG.Application.Services
{
    /// <summary>
    /// ETL console application service class implementation of <see cref="IConsoleAppService" />.
    /// </summary>
    /// <param name="embeddingGenerator">
    /// <see cref="IEmbeddingGenerator" />.
    /// </param>
    /// <param name="repository">
    /// <see cref="IArticleEmbeddingRepository" />.
    /// </param>
	/// <param name="newsApiClient">
	/// <see cref="IExternalArticleApiClient" />.
	/// </param>
    public class ETLConsoleAppService(
		IEmbeddingGenerator embeddingGenerator, IArticleEmbeddingRepository repository, IExternalArticleApiClient newsApiClient) : IConsoleAppService
	{
		private readonly IEmbeddingGenerator _embeddingGenerator = embeddingGenerator;
		private readonly IArticleEmbeddingRepository _repository = repository;
		private readonly IExternalArticleApiClient _newsApiClient = newsApiClient;

		/// <inheritdoc cref="IConsoleAppService.RunAsync" />
		public async Task RunAsync()
		{
			try
			{
				var maxArticleDate = DateTime.UtcNow.AddDays(-30).Date;

				var articles = await GetArticlesAsync(maxArticleDate);

				var articleEmbeddings = await GetEmbeddingsForArticleDescriptionsAsync(articles);

				await BulkInsertEmbeddingsAsync(maxArticleDate, articleEmbeddings);
			} 
			catch (Exception ex)
			{
				Console.WriteLine($"Encountered an exception - {ex.Message}");
			}
		}

		private async Task<IList<NewsApiV2Article>> GetArticlesAsync(DateTime maxArticleDate)
		{
			var response = 
				await _newsApiClient.GetArticlesAsync<NewsApiV2Response>(
					$"everything?qInTitle=philadelphia eagles&sortBy=publishedAt&from={maxArticleDate:yyyy-MM-dd}");

			return response.Articles ?? [];
		}

		private async Task<IList<ArticleEmbeddingRecord>> GetEmbeddingsForArticleDescriptionsAsync(IList<NewsApiV2Article> articles)
		{
			var articleEmbeddings = new List<ArticleEmbeddingRecord>();
			foreach (var article in articles)
			{
				if (article == null || article.Description == null) { continue; }

				var embeddings = await _embeddingGenerator.GenerateEmbeddingsAsync([article.Description]);
				var embedding = embeddings.FirstOrDefault();

				var articleEmbedding = new ArticleEmbeddingRecord
				{
					Content = article.Description,
					Embedding = embedding.ToArray()
				};

				articleEmbeddings.Add(articleEmbedding);
			}

			return articleEmbeddings;
		}

		private async Task BulkInsertEmbeddingsAsync(DateTime maxArticleDate, IList<ArticleEmbeddingRecord> articleEmbeddings)
		{
			await _repository.DeleteAllEmbeddingsAsync(maxArticleDate);
			await _repository.BulkInsertEmbeddingsAsync(articleEmbeddings);
		}
	}
}