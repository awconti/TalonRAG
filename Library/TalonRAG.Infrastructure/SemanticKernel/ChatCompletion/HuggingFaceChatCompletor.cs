using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.HuggingFace;
using TalonRAG.Domain.Configuration;
using TalonRAG.Domain.Extensions;

namespace TalonRAG.Infrastructure.SemanticKernel.ChatCompletion
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
        public async Task<string?> GetChatMessageContentAsync(TalonRAGChatHistory chatHistory)
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

            var chatMessageContent = await chatCompletionService.GetChatMessageContentAsync(chatHistory, executionSettings);
            return chatMessageContent?.Content;
        }
    }
}
