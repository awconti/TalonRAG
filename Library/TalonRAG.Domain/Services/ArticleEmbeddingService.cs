using TalonRAG.Domain.Entities;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Domain.Models;

namespace TalonRAG.Domain.Services
{
	/// <summary>
	/// Article embedding service class implementation of <see cref="IArticleEmbeddingService" />.
	/// </summary>
	/// <param name="embeddingGenerationService">
	/// <see cref="IEmbeddingGenerationService" />.
	/// </param>
	/// <param name="repository">
	/// <see cref="IArticleEmbeddingRepository" />.
	/// </param>
	public class ArticleEmbeddingService(IEmbeddingGenerationService embeddingGenerationService, IArticleEmbeddingRepository repository) : IArticleEmbeddingService
	{
		private readonly IEmbeddingGenerationService _embeddingGenerationService = embeddingGenerationService;
		private readonly IArticleEmbeddingRepository _repository = repository;

		/// <inheritdoc cref="IArticleEmbeddingService.CreateArticleDescriptionsAsEmbeddingsAsync(IList{string}, DateTime)" />
		public async Task CreateEmbeddingsForArticleDescriptionsAsync(IList<string> articleDescriptions, DateTime maxArticleDate)
		{
			var articleEmbeddingRecords = new List<ArticleEmbeddingRecord>();
			foreach (var description in articleDescriptions)
			{
				var embeddings = await _embeddingGenerationService.GenerateEmbeddingsAsync([description]);
				var embedding = embeddings.FirstOrDefault();

				var articleEmbedding = new ArticleEmbeddingRecord
				{
					Content = description,
					Embedding = embedding.ToArray()
				};

				articleEmbeddingRecords.Add(articleEmbedding);
			}

			await _repository.DeleteAllEmbeddingsAsync(maxArticleDate);
			await _repository.BulkInsertEmbeddingsAsync(articleEmbeddingRecords);
		}

		/// <inheritdoc cref="IArticleEmbeddingService.GetSimilarArticleEmbeddingsForMessageContentAsync(string)" />
		public async Task<IList<ArticleEmbedding>> GetSimilarArticleEmbeddingsForMessageContentAsync(string messageContent)
		{
			var messageContentEmbeddings = await _embeddingGenerationService.GenerateEmbeddingsAsync([ messageContent ]);
			var articleEmbeddingRecords = await _repository.GetSimilarEmbeddingsAsync([ ..messageContentEmbeddings.FirstOrDefault().ToArray() ]);

			var articleEmbeddings = new List<ArticleEmbedding>();
			foreach(var record in articleEmbeddingRecords)
			{
				articleEmbeddings.Add(new ArticleEmbedding { ArticleEmbeddingRecord = record });
			}

			return articleEmbeddings;
		}
	}
}
