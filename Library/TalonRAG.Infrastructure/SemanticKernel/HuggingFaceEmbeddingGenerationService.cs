using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Infrastructure.ConfigurationSettings;
using TalonRAG.Infrastructure.Extensions;

namespace TalonRAG.Infrastructure.SemanticKernel
{
    /// <summary>
    /// HuggingFace specific implementation of <see cref="IEmbeddingGenerationService"/>.
    /// </summary>
    /// <param name="options">
    /// <see cref="IOptions{EmbeddingGeneratorConfigurationSettings}"/>.
    /// </param>
    public class HuggingFaceEmbeddingGenerationService(IOptions<EmbeddingGenerationConfigurationSettings> options) : IEmbeddingGenerationService
    {
        private readonly EmbeddingGenerationConfigurationSettings _configurationSettings = options.Value;

        /// <inheritdoc cref="IEmbeddingGenerationService.GenerateEmbeddingsAsync(IList{string})"/>
        public async Task<IList<ReadOnlyMemory<float>>> GenerateEmbeddingsAsync(IList<string> text)
        {
            if (_configurationSettings.IsMissing())
            {
                throw new Exception("Model configuration settings unknown.");
            }

            var kernelBuilder = Kernel.CreateBuilder();
            kernelBuilder.Services.AddHuggingFaceTextEmbeddingGeneration(
                    model: _configurationSettings.ModelId,
                    apiKey: _configurationSettings?.ApiKey);

            var kernel = kernelBuilder.Build();

            var embeddingGenerator = kernel.GetRequiredService<ITextEmbeddingGenerationService>();
            return await embeddingGenerator.GenerateEmbeddingsAsync(text);
        }
    }
}
