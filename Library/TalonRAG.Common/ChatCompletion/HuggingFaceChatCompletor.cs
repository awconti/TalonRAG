using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.HuggingFace;
using TalonRAG.Common.Configuration;
using TalonRAG.Common.Extensions;

namespace TalonRAG.Common.ChatCompletion
{
	/// <summary>
	/// HuggingFace specific implementation of <see cref="IChatCompletor"/>.
	/// </summary>
	/// <param name="configurationSettings">
	/// <see cref="ChatCompletorConfigurationSettings"/>.
	/// </param>
	public class HuggingFaceChatCompletor(IOptions<ChatCompletorConfigurationSettings> configurationSettings) : IChatCompletor
	{
		private readonly ChatCompletorConfigurationSettings _configurationSettings = configurationSettings.Value;

		/// <inheritdoc cref="IChatCompletor.GetChatMessageContentAsync(string)" />
		public async Task<ChatMessageContent> GetChatMessageContentAsync(string prompt)
		{
			if (_configurationSettings.IsMissing())
			{
				throw new Exception("Model configuration settings unknown.");
			}

			var builder = Kernel.CreateBuilder();
			builder.Services.AddHuggingFaceChatCompletion(
				model: _configurationSettings.ModelId,
				apiKey: _configurationSettings.ApiKey);

			var kernel = builder.Build();

			var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
			var executionSettings = new HuggingFacePromptExecutionSettings
			{
				MaxTokens = 1200
			};

			return await chatCompletionService.GetChatMessageContentAsync(prompt, executionSettings);
		}
	}
}
