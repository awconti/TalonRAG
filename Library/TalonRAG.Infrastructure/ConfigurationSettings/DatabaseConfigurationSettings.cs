namespace TalonRAG.Infrastructure.ConfigurationSettings
{
	/// <summary>
	/// Configuration settings used in the context of a database.
	/// </summary>
	public class DatabaseConfigurationSettings
	{
		/// <summary>
		/// Represents the connection string to use when connecting to database.
		/// </summary>
		public string? Connection { get; set; }
	}
}
