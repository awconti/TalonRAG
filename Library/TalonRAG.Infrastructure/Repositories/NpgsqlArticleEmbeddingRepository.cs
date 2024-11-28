﻿using Microsoft.Extensions.Options;
using Pgvector;
using TalonRAG.Domain.Entities;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Infrastructure.ConfigurationSettings;

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
			if (createDate != null)
			{
				sql += "WHERE create_date AT TIME ZONE 'UTC' > @CreateDate;";
				parameters.Add("@CreateDate", createDate);
			}

			await ExecuteNonQueryAsync(sql, parameters);
		}

		/// <inheritdoc cref="IArticleEmbeddingRepository.InsertEmbeddingsAsync(IList{ArticleEmbeddingRecord})" />
		public async Task InsertEmbeddingsAsync(IList<ArticleEmbeddingRecord> embeddings)
		{
			foreach (var embedding in embeddings)
			{
				var sql =
					"INSERT INTO article_embeddings (article_embedding, article_content) VALUES (@Embedding, @Content);";

				var parameters = new Dictionary<string, object>
				{
					{ "@Embedding", new Vector(embedding.Embedding) },
					{ "@Content", embedding.Content }
				};

				await ExecuteNonQueryAsync(sql, parameters);
			}
		}

		/// <inheritdoc cref="IArticleEmbeddingRepository.BulkInsertEmbeddingsAsync(IList{ArticleEmbeddingRecord})" />
		public async Task BulkInsertEmbeddingsAsync(IList<ArticleEmbeddingRecord> embeddings)
		{
			var command =
				"COPY article_embeddings (article_embedding, article_content) FROM STDIN (FORMAT BINARY)";

			await BinaryImportAsync(command, async writer =>
			{
				foreach (var embedding in embeddings)
				{
					await writer.StartRowAsync();
					writer.Write(new Vector(embedding.Embedding));
					writer.Write(embedding.Content);
				}
			});
		}

		/// <inheritdoc cref="IArticleEmbeddingRepository.GetSimilarEmbeddingsAsync(float[], int)" />
		public async Task<IList<ArticleEmbeddingRecord>> GetSimilarEmbeddingsAsync(float[] embedding, int limit = 3)
		{
			string sql = $@"
                SELECT id, article_embedding, article_content
				FROM article_embeddings
				ORDER BY article_embedding <#> @Embedding
				LIMIT @Limit;
			";

			var parameters = new Dictionary<string, object>
			{
				{ "@Embedding", new Vector(embedding) },
				{ "@Limit", limit }
			};

			return await ExecuteReaderAsync(
				sql,
				reader => new ArticleEmbeddingRecord
				{
					Id = reader.GetInt32(reader.GetOrdinal("id")),
					Embedding = reader.GetFieldValue<Vector>(reader.GetOrdinal("article_embedding")).ToArray(),
					Content = reader.GetString(reader.GetOrdinal("article_content"))
				},
				parameters);
		}
	}
}
