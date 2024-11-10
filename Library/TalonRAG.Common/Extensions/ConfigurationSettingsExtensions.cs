using TalonRAG.Common.Configuration;

namespace TalonRAG.Common.Extensions
{
	/// <summary>
	/// Extensions tailored to configuration settings.
	/// </summary>
	internal static class ConfigurationSettingsExtensions
	{
		/// <summary>
		/// Determines whether or not model configuration properties are properly set.
		/// </summary>
		/// <param name="modelConfigurationSettings">
		/// <see cref="ModelConfigurationSettings"/>.
		/// </param>
		internal static bool IsMissing(this ModelConfigurationSettings modelConfigurationSettings)
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
		internal static bool IsMissing(this DatabaseConfigurationSettings databaseConfigurationSettings)
		{
			return databaseConfigurationSettings == null ||
				databaseConfigurationSettings.Connection == null;
		}
	}
}
