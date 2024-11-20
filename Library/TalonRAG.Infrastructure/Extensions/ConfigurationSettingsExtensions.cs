using TalonRAG.Infrastructure.ConfigurationSettings;

namespace TalonRAG.Infrastructure.Extensions
{
	/// <summary>
	/// Extensions tailored to configuration settings.
	/// </summary>
	public static class ConfigurationSettingsExtensions
	{
		/// <summary>
		/// Determines whether or not model configuration properties are properly set.
		/// </summary>
		/// <param name="modelConfigurationSettings">
		/// <see cref="ModelConfigurationSettings"/>.
		/// </param>
		public static bool IsMissing(this ModelConfigurationSettings modelConfigurationSettings)
		{
			return modelConfigurationSettings == null ||
				modelConfigurationSettings.ModelId == null ||
				modelConfigurationSettings.ApiKey == null;
		}

		/// <summary>
		/// Determines whether or not database configuration properties are properly set.
		/// </summary>
		/// <param name="databaseConfigurationSettings">
		/// <see cref="DatabaseConfigurationSettings"/>.
		/// </param>
		public static bool IsMissing(this DatabaseConfigurationSettings databaseConfigurationSettings)
		{
			return databaseConfigurationSettings == null ||
				databaseConfigurationSettings.Connection == null;
		}
	}
}
