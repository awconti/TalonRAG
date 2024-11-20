using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;
using TalonRAG.Infrastructure.ConfigurationSettings;
using TalonRAG.Infrastructure.Extensions;

namespace TalonRAG.Infrastructure.SemanticKernel.Embedding
{
	/// <summary>
	/// HuggingFace specific implementation of <see cref="IEmbeddingGenerator"/>.
	/// </summary>
	/// <param name="configurationSettings">
	/// <see cref="EmbeddingGeneratorConfigurationSettings"/>.
	/// </param>
	public class HuggingFaceEmbeddingGenerator(IOptions<EmbeddingGeneratorConfigurationSettings> configurationSettings) : IEmbeddingGenerator
	{
		private readonly EmbeddingGeneratorConfigurationSettings _configurationSettings = configurationSettings.Value;

		/// <inheritdoc cref="IEmbeddingGenerator.GenerateEmbeddingsAsync(IList{string})"/>
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
