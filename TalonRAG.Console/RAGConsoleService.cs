using Microsoft.SemanticKernel;
using TalonRAG.Common.ChatCompletion;
using TalonRAG.Common.Embedding;
using TalonRAG.Common.Persistence.DTO;
using TalonRAG.Common.Persistence.Repository;

internal class RAGConsoleService(
	IChatCompletor chatCompletor, IEmbeddingGenerator embeddingGenerator, IEmbeddingRepository repository)
{
	private readonly IChatCompletor _chatCompletor = chatCompletor;
	private readonly IEmbeddingGenerator _embeddingGenerator = embeddingGenerator;
	private readonly IEmbeddingRepository _repository = repository;

	public async Task RunAsync()
	{
		try
		{
			while (true)
			{
				Console.Write("You: ");
				string? userInput = Console.ReadLine();

				Console.ForegroundColor = ConsoleColor.DarkGreen;

				if (userInput == null || userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
				{
					Console.WriteLine("TalonRAG: Goodbye!");
					break;
				}

				var inputEmbedding = await GenerateEmbeddingForInput(userInput);
				var similarArticleEmbeddings = await GetSimilarArticleEmbeddings(inputEmbedding);
				var response = await GeneratePrompt(userInput, similarArticleEmbeddings.First());
				Console.WriteLine($"TalonRAG: {response.Content}");

                Console.ResetColor();
            }
		} catch (Exception ex) 
		{ 
			Console.WriteLine($"TalonRAG: Encountered an exception - {ex.Message}");
		} finally
		{
			Console.ResetColor();
		}
	}

	private async Task<IList<float>> GenerateEmbeddingForInput(string input)
	{
		var embeddings = await _embeddingGenerator.GenerateEmbeddingsAsync([input]);
		var embedding = embeddings.FirstOrDefault();

		return embedding.ToArray();
	}

	private async Task<IEnumerable<ArticleEmbedding>> GetSimilarArticleEmbeddings(IList<float> embedding)
	{
		return await _repository.GetSimilarEmbeddingsAsync([.. embedding]);
	}

	private async Task<ChatMessageContent> GeneratePrompt(string input, ArticleEmbedding articleEmbedding)
	{
		string promptTemplate = $@"
			You're an AI assistant called TalonRAG who retrieves the latest news article descriptions for fans of the NFL team, Philadelphia Eagles.
			Based on the similar content, you formulate an informative response based on the similar content you receive using the user's response.

			Similar Content: {articleEmbedding.Content}

			User's Response: {input}";

		return await _chatCompletor.GetChatMessageContentAsync(promptTemplate);
	}
}
