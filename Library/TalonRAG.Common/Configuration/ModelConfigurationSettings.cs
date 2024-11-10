namespace TalonRAG.Common.Configuration
{
	/// <summary>
	/// Generic configurations settings used when interacting with a specific model via Semantic Kernel.
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
