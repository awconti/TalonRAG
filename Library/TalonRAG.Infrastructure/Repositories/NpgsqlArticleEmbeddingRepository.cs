using Microsoft.Extensions.Options;
using Pgvector;
using TalonRAG.Domain.Entities;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Infrastructure.ConfigurationSettings;

namespace TalonRAG.Infrastructure.Repositories
{
	/// <summary>
	/// Npgsql specific embedding repository implementation of <see cref="IEmbeddingRepository"/>.
	/// </summary>
	/// <param name="options">
	/// <see cref="IOptions{DatabaseConfigurationSettings}"/>.
	/// </param>
	public class NpgsqlArticleEmbeddingRepository(IOptions<DatabaseConfigurationSettings> options) : BaseNpgsqlRepository(options), IEmbeddingRepository
	{
		/// <inheritdoc cref="IEmbeddingRepository.DeleteAllEmbeddingsAsync" />
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

		/// <inheritdoc cref="IEmbeddingRepository.InsertEmbeddingsAsync(IList{EmbeddingRecord})" />
		public async Task InsertEmbeddingsAsync(IList<EmbeddingRecord> embeddingRecords)
		{
			foreach (var record in embeddingRecords)
			{
				var sql =
					"INSERT INTO article_embeddings (article_embedding, article_content) VALUES (@Embedding, @Content);";

				var parameters = new Dictionary<string, object>
				{
					{ "@Embedding", new Vector(record.VectorEmbedding) },
					{ "@Content", record.Content }
				};

				await ExecuteNonQueryAsync(sql, parameters);
			}
		}

		/// <inheritdoc cref="IEmbeddingRepository.BulkInsertEmbeddingsAsync(IList{EmbeddingRecord})" />
		public async Task BulkInsertEmbeddingsAsync(IList<EmbeddingRecord> embeddingRecords)
		{
			var command =
				"COPY article_embeddings (article_embedding, article_content) FROM STDIN (FORMAT BINARY)";

			await BinaryImportAsync(command, async writer =>
			{
				foreach (var record in embeddingRecords)
				{
					await writer.StartRowAsync();
					writer.Write(new Vector(record.VectorEmbedding));
					writer.Write(record.Content);
				}
			});
		}

		/// <inheritdoc cref="IEmbeddingRepository.GetSimilarEmbeddingsAsync(float[], int)" />
		public async Task<IList<EmbeddingRecord>> GetSimilarEmbeddingsAsync(float[] embedding, int limit = 3)
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

			return await ExecuteReaderAsync(
				sql,
				reader => new EmbeddingRecord
				{
					Id = reader.GetInt32(reader.GetOrdinal("id")),
					VectorEmbedding = reader.GetFieldValue<Vector>(reader.GetOrdinal("article_embedding")).ToArray(),
					Content = reader.GetString(reader.GetOrdinal("article_content"))
				},
				parameters);
		}
	}
}
