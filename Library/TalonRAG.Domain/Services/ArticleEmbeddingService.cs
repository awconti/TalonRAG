using TalonRAG.Domain.Entities;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Domain.Extensions;
using TalonRAG.Domain.Models;

namespace TalonRAG.Domain.Services
{
	/// <summary>
	/// Article embedding service class implementation of <see cref="IEmbeddingService" />.
	/// </summary>
	/// <param name="embeddingGenerationService">
	/// <see cref="IEmbeddingGenerationService" />.
	/// </param>
	/// <param name="repository">
	/// <see cref="IEmbeddingRepository" />.
	/// </param>
	public class ArticleEmbeddingService(IEmbeddingGenerationService embeddingGenerationService, IEmbeddingRepository repository) : IEmbeddingService
	{
		private readonly IEmbeddingGenerationService _embeddingGenerationService = embeddingGenerationService;
		private readonly IEmbeddingRepository _repository = repository;

		/// <inheritdoc cref="IEmbeddingService.CreateEmbeddingsForContentAsync(IList{string}, DateTime)" />
		public async Task CreateEmbeddingsForContentAsync(IList<string> articleDescriptions, DateTime maxArticleDate)
		{
			var articleEmbeddingRecords = new List<EmbeddingRecord>();
			foreach (var description in articleDescriptions)
			{
				var embeddings = await _embeddingGenerationService.GenerateEmbeddingsAsync([description]);
				var embedding = embeddings.FirstOrDefault();

				var articleEmbedding = new EmbeddingRecord
				{
					Content = description,
					VectorEmbedding = embedding.ToArray()
				};

				articleEmbeddingRecords.Add(articleEmbedding);
			}

			await _repository.DeleteAllEmbeddingsAsync(maxArticleDate);
			await _repository.BulkInsertEmbeddingsAsync(articleEmbeddingRecords);
		}

		/// <inheritdoc cref="IEmbeddingService.GetSimilarEmbeddingsFromContentAsync(string)" />
		public async Task<IList<Embedding>> GetSimilarEmbeddingsFromContentAsync(string content)
		{
			var messageContentEmbeddings = await _embeddingGenerationService.GenerateEmbeddingsAsync([ content ]);
			var articleEmbeddingRecords = await _repository.GetSimilarEmbeddingsAsync([ ..messageContentEmbeddings.FirstOrDefault().ToArray() ]);

			return articleEmbeddingRecords.Select(record => record.ToDomainModel()).ToList();
		}
	}
}
