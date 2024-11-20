namespace TalonRAG.Infrastructure.ConfigurationSettings
{
	/// <summary>
	/// Generic configuration settings used when interacting with a specific model via Microsoft.SemanticKernel.
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
