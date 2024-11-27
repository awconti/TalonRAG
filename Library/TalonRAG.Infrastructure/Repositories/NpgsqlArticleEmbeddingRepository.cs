using Microsoft.Extensions.Options;
using Npgsql;
using Pgvector;
using TalonRAG.Domain.Entities;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Infrastructure.ConfigurationSettings;
using TalonRAG.Infrastructure.Extensions;

namespace TalonRAG.Infrastructure.Repositories
{
	/// <summary>
	/// Npgsql specific embedding repository implementation of <see cref="IArticleEmbeddingRepository"/>.
	/// </summary>
	/// <param name="configurationSettings">
	/// <see cref="DatabaseConfigurationSettings"/>.
	/// </param>
	public class NpgsqlArticleEmbeddingRepository(IOptions<DatabaseConfigurationSettings> configurationSettings) : IArticleEmbeddingRepository
	{
		private readonly DatabaseConfigurationSettings _configurationSettings = configurationSettings.Value;

		/// <inheritdoc cref="IArticleEmbeddingRepository.DeleteAllEmbeddingsAsync" />
		public async Task DeleteAllEmbeddingsAsync()
		{
			using var connection = await CreateConnection();

			var sql = "DELETE FROM article_embeddings;";

			using var command = new NpgsqlCommand(sql, connection);

			await command.ExecuteNonQueryAsync();
		}

		/// <inheritdoc cref="IArticleEmbeddingRepository.InsertEmbeddingsAsync(IList{ArticleEmbedding})" />
		public async Task InsertEmbeddingsAsync(IList<ArticleEmbedding> embeddings)
		{
			using var connection = await CreateConnection();

			foreach (var embedding in embeddings)
			{
				if (embedding == null || embedding.Embedding == null || embedding.Content == null) { continue; }

				var sql =
					"INSERT INTO article_embeddings (article_embedding, article_content) VALUES (@Embedding, @Content);";

				using var command = new NpgsqlCommand(sql, connection);
				var vector = new Vector(embedding.Embedding);
				command.Parameters.AddWithValue("@Embedding", vector);
				command.Parameters.AddWithValue("@Content", embedding.Content);

				await command.ExecuteNonQueryAsync();
			}
		}

		/// <inheritdoc cref="IArticleEmbeddingRepository.BulkInsertEmbeddingsAsync(IList{ArticleEmbedding})" />
		public async Task BulkInsertEmbeddingsAsync(IList<ArticleEmbedding> embeddings)
		{
			using var connection = await CreateConnection();

			var command =
				"COPY article_embeddings (article_embedding, article_content) FROM STDIN (FORMAT BINARY)";

			using var writer = await connection.BeginBinaryImportAsync(command);
			foreach (var embedding in embeddings)
			{
				writer.StartRow();
				writer.Write(new Vector(embedding.Embedding));
				writer.Write(embedding.Content);
			}

			writer.Complete();
		}

		/// <inheritdoc cref="IArticleEmbeddingRepository.GetSimilarEmbeddingsAsync(float[], int)" />
		public async Task<IList<ArticleEmbedding>> GetSimilarEmbeddingsAsync(float[] embedding, int limit = 3)
		{
			var similarArticleEmbeddings = new List<ArticleEmbedding>();

			await using var connection = await CreateConnection();

			string sql = $@"
                SELECT id, article_embedding, article_content
				FROM article_embeddings
				ORDER BY article_embedding <#> @Embedding
				LIMIT @Limit;
			";

			using var command = new NpgsqlCommand(sql, connection);
			var vector = new Vector(embedding);
			command.Parameters.AddWithValue("@Embedding", vector);
			command.Parameters.AddWithValue("@Limit", limit);

			await using var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				similarArticleEmbeddings.Add(new ArticleEmbedding
				{
					Id = reader.GetInt32(reader.GetOrdinal("id")),
					Embedding = reader.GetFieldValue<Vector>(reader.GetOrdinal("article_embedding")).ToArray(),
					Content = reader.GetString(reader.GetOrdinal("article_content"))
				});
			}

			return similarArticleEmbeddings;
		}

		private async Task<NpgsqlConnection> CreateConnection()
		{
			if (_configurationSettings.IsMissing())
			{
				throw new Exception("Database configuration settings unknown.");
			}

			var dataSourceBuilder = new NpgsqlDataSourceBuilder(_configurationSettings.Connection);
			dataSourceBuilder.UseVector();

			await using var dataSource = dataSourceBuilder.Build();
			return await dataSource.OpenConnectionAsync();
		}
	}
}
