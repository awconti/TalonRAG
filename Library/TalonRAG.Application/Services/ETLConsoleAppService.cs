using System.Text.Json;
using TalonRAG.Application.DataTransferObjects;
using TalonRAG.Application.Interfaces;
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
    /// <see cref="IEmbeddingRepository" />.
    /// </param>
    public class ETLConsoleAppService(
		IEmbeddingGenerator embeddingGenerator, IEmbeddingRepository repository) : IConsoleAppService
	{
		private readonly IEmbeddingGenerator _embeddingGenerator = embeddingGenerator;
		private readonly IEmbeddingRepository _repository = repository;

		/// <inheritdoc cref="IConsoleAppService.RunAsync" />
		public async Task RunAsync()
		{
			var articles = await GetArticles();

			var articleEmbeddings = await GetEmbeddingsForArticleDescriptions(articles);

			await BulkInsertEmbeddings(articleEmbeddings);
		}

		private static async Task<List<Article>> GetArticles()
		{
			string filePath = "./NewsApiArticles.json";
			string jsonString = await File.ReadAllTextAsync(filePath);

			var articles = JsonSerializer.Deserialize<List<Article>>(jsonString);

			return articles ?? [];
		}

		private async Task<IList<ArticleEmbedding>> GetEmbeddingsForArticleDescriptions(IList<Article> articles)
		{
			var articleEmbeddings = new List<ArticleEmbedding>();
			foreach (var article in articles)
			{
				if (article == null || article.Description == null) { continue; }

				var embeddings = await _embeddingGenerator.GenerateEmbeddingsAsync([article.Description]);
				var embedding = embeddings.FirstOrDefault();

				var articleEmbedding = new ArticleEmbedding
				{
					Content = article.Description,
					Embedding = embedding.ToArray()
				};

				articleEmbeddings.Add(articleEmbedding);
			}

			return articleEmbeddings;
		}

		private async Task BulkInsertEmbeddings(IList<ArticleEmbedding> articleEmbeddings)
		{
			await _repository.DeleteAllEmbeddingsAsync();
			await _repository.BulkInsertEmbeddingsAsync(articleEmbeddings);
		}
	}
}