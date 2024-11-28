using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.HuggingFace;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Domain.Models;
using TalonRAG.Infrastructure.ConfigurationSettings;
using TalonRAG.Infrastructure.Extensions;

namespace TalonRAG.Infrastructure.SemanticKernel
{
    /// <summary>
    /// HuggingFace specific implementation of <see cref="IChatCompletor"/>.
    /// </summary>
    /// <param name="options">
    /// <see cref="IOptions{ChatCompletorConfigurationSettings}"/>.
    /// </param>
    public class HuggingFaceChatCompletor(IOptions<ChatCompletorConfigurationSettings> options) : IChatCompletor
    {
        private readonly ChatCompletorConfigurationSettings _configurationSettings = options.Value;

        /// <inheritdoc cref="IChatCompletor.GetChatMessageContentAsync(Conversation)" />
        public async Task<string?> GetChatMessageContentAsync(Conversation conversation)
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

			var chatHistory = conversation.ToChatHistory();

			var executionSettings = new HuggingFacePromptExecutionSettings
            {
                MaxTokens = 1200
            };

            var chatMessageContent = await chatCompletionService.GetChatMessageContentAsync(chatHistory, executionSettings);
            return chatMessageContent?.Content;
        }
    }
}
