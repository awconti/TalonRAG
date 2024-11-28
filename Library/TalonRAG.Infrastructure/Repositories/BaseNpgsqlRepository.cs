using Microsoft.Extensions.Options;
using Npgsql;
using TalonRAG.Infrastructure.ConfigurationSettings;
using TalonRAG.Infrastructure.Extensions;

namespace TalonRAG.Infrastructure.Repositories
{
	/// <summary>
	/// Base implementation for classes seeking to utilize Npgsql for data access.
	/// </summary>
	/// <param name="options">
	/// <see cref="IOptions{DatabaseConfigurationSettings}"/>.
	/// </param>
	public abstract class BaseNpgsqlRepository(IOptions<DatabaseConfigurationSettings> options)
	{
		private readonly DatabaseConfigurationSettings _configurationSettings = options.Value;

		/// <summary>
		/// <see cref="System.Data.Common.DbCommand.ExecuteNonQueryAsync()"/>
		/// </summary>
		/// <param name="sql">
		/// The SQL to execute.
		/// </param>
		/// <param name="parameters">
		/// Optional parameters to define when executing SQL.
		/// </param>
		protected async Task<int> ExecuteNonQueryAsync(string sql, IDictionary<string, object>? parameters = null)
		{
			using var connection = await CreateConnection();
			using var command = new NpgsqlCommand(sql, connection);
			AddParameters(command, parameters);

			return await command.ExecuteNonQueryAsync();
		}

		/// <summary>
		/// <see cref="System.Data.Common.DbCommand.ExecuteScalarAsync()"/>
		/// </summary>
		/// <param name="sql">
		/// The SQL to execute.
		/// </param>
		/// <param name="parameters">
		/// Optional parameters to define when executing SQL.
		/// </param>
		protected async Task<T?> ExecuteScalarAsync<T>(string sql, IDictionary<string, object>? parameters = null)
		{
			using var connection = await CreateConnection();
			using var command = new NpgsqlCommand(sql, connection);
			AddParameters(command, parameters);

			var result = await command.ExecuteScalarAsync();
			return result == DBNull.Value ? default : (T?)result;
		}

		/// <summary>
		/// <see cref="System.Data.Common.DbCommand.ExecuteReaderAsync()"/>
		/// </summary>
		/// <param name="sql">
		/// The SQL to execute.
		/// </param>
		/// <param name="map">
		/// Function delegate to map columns in each record to an object.
		/// </param>
		/// <param name="parameters">
		/// Optional parameters to define when executing SQL.
		/// </param>
		protected async Task<IList<T>> ExecuteReaderAsync<T>(string sql, Func<NpgsqlDataReader, T> map, IDictionary<string, object>? parameters = null)
		{
			var results = new List<T>();

			using var connection = await CreateConnection();
			using var command = new NpgsqlCommand(sql, connection);
			AddParameters(command, parameters);

			await using var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				results.Add(map(reader));
			}

			return results;
		}

		/// <summary>
		/// <see cref="NpgsqlConnection.BeginBinaryImportAsync(string, CancellationToken)"/>
		/// </summary>
		/// <param name="command">
		/// The command to execute.
		/// </param>
		/// <param name="writeAction">
		/// Function delegate to define what is written at the row level.
		/// </param>
		public async Task BinaryImportAsync(string command, Func<NpgsqlBinaryImporter, Task> writeAction)
		{
			using var connection = await CreateConnection();
			await using var writer = await connection.BeginBinaryImportAsync(command);

			try
			{
				await writeAction(writer);
				await writer.CompleteAsync();
			}
			catch
			{
				await writer.CloseAsync();
				throw;
			}
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

		private static void AddParameters(NpgsqlCommand command, IDictionary<string, object>? parameters)
		{
			if (parameters == null) return;

			foreach (var param in parameters)
			{
				command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
			}
		}
	}
}