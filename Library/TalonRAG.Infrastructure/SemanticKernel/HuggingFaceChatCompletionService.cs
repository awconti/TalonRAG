using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.HuggingFace;
using TalonRAG.Domain.Models;
using TalonRAG.Infrastructure.ConfigurationSettings;
using TalonRAG.Infrastructure.Extensions;
using IChatCompletionService = TalonRAG.Domain.Interfaces.IChatCompletionService;

namespace TalonRAG.Infrastructure.SemanticKernel
{
    /// <summary>
    /// HuggingFace specific implementation of <see cref="IChatCompletionService"/>.
    /// </summary>
    /// <param name="options">
    /// <see cref="IOptions{ChatCompletionConfigurationSettings}"/>.
    /// </param>
    public class HuggingFaceChatCompletionService(IOptions<ChatCompletionConfigurationSettings> options) : IChatCompletionService
    {
        private readonly ChatCompletionConfigurationSettings _configurationSettings = options.Value;

        /// <inheritdoc cref="IChatCompletionService.GetChatMessageContentAsync(ConversationModel)" />
        public async Task<string> GetChatMessageContentAsync(ConversationModel conversation)
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

            var chatCompletionService = kernel.GetRequiredService<Microsoft.SemanticKernel.ChatCompletion.IChatCompletionService>();

			var chatHistory = conversation.ToChatHistory();

			var executionSettings = new HuggingFacePromptExecutionSettings
            {
                MaxTokens = 1200
            };

            var chatMessageContent = await chatCompletionService.GetChatMessageContentAsync(chatHistory, executionSettings);
            return chatMessageContent.Content ?? string.Empty;
        }
    }
}
