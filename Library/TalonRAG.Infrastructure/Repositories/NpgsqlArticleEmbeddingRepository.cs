using Microsoft.Extensions.Options;
using Pgvector;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Domain.Models;
using TalonRAG.Infrastructure.ConfigurationSettings;
using TalonRAG.Infrastructure.Entities;
using TalonRAG.Infrastructure.Extensions;

namespace TalonRAG.Infrastructure.Repositories
{
	/// <summary>
	/// Npgsql specific embedding repository implementation of <see cref="IArticleEmbeddingRepository"/>.
	/// </summary>
	/// <param name="options">
	/// <see cref="IOptions{DatabaseConfigurationSettings}"/>.
	/// </param>
	public class NpgsqlArticleEmbeddingRepository(IOptions<DatabaseConfigurationSettings> options) : BaseNpgsqlRepository(options), IArticleEmbeddingRepository
	{
		/// <inheritdoc cref="IArticleEmbeddingRepository.DeleteAllEmbeddingsAsync" />
		public async Task DeleteAllEmbeddingsAsync(DateTime? createDate = null)
		{
			var sql = "DELETE FROM article_embeddings ";

			var parameters = new Dictionary<string, object>();
			if (createDate.HasValue)
			{
				sql += "WHERE create_date >= @CreateDate;";
				parameters.Add("@CreateDate", createDate.Value.Date);
			}

			await ExecuteNonQueryAsync(sql, parameters);
		}

		/// <inheritdoc cref="IArticleEmbeddingRepository.InsertEmbeddingsAsync(IList{ArticleEmbeddingModel})" />
		public async Task InsertEmbeddingsAsync(IList<ArticleEmbeddingModel> embeddingModels)
		{
			foreach (var model in embeddingModels)
			{
				var sql =
					"INSERT INTO article_embeddings (article_embedding, article_content) VALUES (@Embedding, @Content);";

				var parameters = new Dictionary<string, object>
				{
					{ "@Embedding", new Vector(model.VectorEmbedding) },
					{ "@Content", model.Content }
				};

				await ExecuteNonQueryAsync(sql, parameters);
			}
		}

		/// <inheritdoc cref="IArticleEmbeddingRepository.BulkInsertEmbeddingsAsync(IList{ArticleEmbeddingModel})" />
		public async Task BulkInsertEmbeddingsAsync(IList<ArticleEmbeddingModel> embeddingModels)
		{
			var command =
				"COPY article_embeddings (article_embedding, article_content) FROM STDIN (FORMAT BINARY)";

			await BinaryImportAsync(command, async writer =>
			{
				foreach (var model in embeddingModels)
				{
					await writer.StartRowAsync();
					writer.Write(new Vector(model.VectorEmbedding));
					writer.Write(model.Content);
				}
			});
		}

		/// <inheritdoc cref="IArticleEmbeddingRepository.GetSimilarEmbeddingsAsync(float[], int)" />
		public async Task<IList<ArticleEmbeddingModel>> GetSimilarEmbeddingsAsync(float[] embedding, int limit = 3)
		{
			string sql = $@"
                SELECT id, article_embedding, article_content
				FROM article_embeddings
				ORDER BY article_embedding <=> @Embedding
				LIMIT @Limit;
			";

			var parameters = new Dictionary<string, object>
			{
				{ "@Embedding", new Vector(embedding) },
				{ "@Limit", limit }
			};

			var embeddingEntities = await ExecuteReaderAsync(
				sql,
				reader => new ArticleEmbeddingEntity
				{
					Id = reader.GetInt32(reader.GetOrdinal("id")),
					VectorEmbedding = reader.GetFieldValue<Vector>(reader.GetOrdinal("article_embedding")).ToArray(),
					Content = reader.GetString(reader.GetOrdinal("article_content"))
				},
				parameters);

			return embeddingEntities.Select(entity => entity.ToDomainModel()).ToList();
		}
	}
}
