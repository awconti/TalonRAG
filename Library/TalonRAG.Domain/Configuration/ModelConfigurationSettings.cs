namespace TalonRAG.Domain.Configuration
{
	/// <summary>
	/// Generic configuration settings used when interacting with a specific model.
	/// </summary>
	public class ModelConfigurationSettings
	{
		/// <summary>
		/// ID for the model to use.
		/// </summary>
		public string? ModelId { get; set; }

		/// <summary>
		/// API key to use.
		/// </summary>
		public string? ApiKey { get; set; }
	}
}
