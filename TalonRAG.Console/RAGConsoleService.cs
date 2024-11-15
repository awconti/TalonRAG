using TalonRAG.Common.ChatCompletion;
using TalonRAG.Common.Domain.DTO;
using TalonRAG.Common.Embedding;
using TalonRAG.Common.Persistence.Repository;

internal class RAGConsoleService(
	IChatCompletor chatCompletor, IEmbeddingGenerator embeddingGenerator, IEmbeddingRepository repository)
{
	private readonly IChatCompletor _chatCompletor = chatCompletor;
	private readonly IEmbeddingGenerator _embeddingGenerator = embeddingGenerator;
	private readonly IEmbeddingRepository _repository = repository;

	private const string SYSTEM_MESSAGE = $@"
		You're an AI assistant called TalonRAG who retrieves the latest news article descriptions for fans of the NFL team, Philadelphia Eagles.
		Based on the similar content, you formulate an informative response based on the similar content you receive using the user's response.
		Only respond with what is given to you as similar content and don't provide any similar content if a question from the user's
		response isn't asked.";

	public async Task RunAsync()
	{
		try
		{
			var chatHistory = new TalonRAGChatHistory(SYSTEM_MESSAGE);

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

				var toolMessage = similarArticleEmbeddings.FirstOrDefault()?.Content ?? "";
				chatHistory.AddToolMessage(toolMessage);
				chatHistory.AddUserMessage(userInput);

				var content = await GenerateChatMessageContent(chatHistory);
				Console.WriteLine($"TalonRAG: {content}");

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

	private async Task<string?> GenerateChatMessageContent(TalonRAGChatHistory chatHistory)
	{
		var content = await _chatCompletor.GetChatMessageContentAsync(chatHistory);
		chatHistory.AddAssistantMessage(content ?? "");

		return content;
	}
}
