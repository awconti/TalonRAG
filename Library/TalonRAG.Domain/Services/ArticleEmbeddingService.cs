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

		/// <inheritdoc cref="IArticleEmbeddingService.CreateEmbeddingsForContentAsync(IList{string}, DateTime)" />
		public async Task CreateEmbeddingsForContentAsync(IList<string> articleDescriptions, DateTime maxArticleDate)
		{
			var articleEmbeddings = new List<ArticleEmbeddingModel>();
			foreach (var description in articleDescriptions)
			{
				var embeddings = await _embeddingGenerationService.GenerateEmbeddingsAsync([description]);
				var embedding = embeddings.FirstOrDefault();

				var articleEmbedding = new ArticleEmbeddingModel
				{
					Content = description,
					VectorEmbedding = embedding.ToArray()
				};

				articleEmbeddings.Add(articleEmbedding);
			}

			await _repository.DeleteAllEmbeddingsAsync(maxArticleDate);
			await _repository.BulkInsertEmbeddingsAsync(articleEmbeddings);
		}

		/// <inheritdoc cref="IArticleEmbeddingService.GetSimilarEmbeddingsFromContentAsync(string)" />
		public async Task<IList<ArticleEmbeddingModel>> GetSimilarEmbeddingsFromContentAsync(string content)
		{
			var messageContentEmbeddings = await _embeddingGenerationService.GenerateEmbeddingsAsync([ content ]);
			return await _repository.GetSimilarEmbeddingsAsync([ ..messageContentEmbeddings.FirstOrDefault().ToArray() ]);
		}
	}
}
