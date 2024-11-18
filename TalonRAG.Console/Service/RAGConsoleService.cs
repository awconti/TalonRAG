using TalonRAG.Domain.Entity;
using TalonRAG.Domain.Model;
using TalonRAG.Domain.SemanticKernel.ChatCompletion;
using TalonRAG.Domain.SemanticKernel.Embedding;
using TalonRAG.Infrastructure.Repository;

namespace TalonRAG.Console.Service
{
	internal class RAGConsoleService(
		IChatCompletor chatCompletor, IEmbeddingGenerator embeddingGenerator, IEmbeddingRepository repository)
	{
		private const string SYSTEM_MESSAGE = $@"
			You're an AI assistant called TalonRAG who retrieves the latest news article descriptions for fans of the NFL team, Philadelphia Eagles.
			Based on the descriptions of latest news articles you retrieve, you formulate an informative response based only on the descriptions of latest news articles.
			Don't report anything as fact unless you retrieve it from an article description provided to you as a tool message.
			Don't help the user with anything other than latest news article descriptions on the Philadelphia Eagles.";

		private readonly IChatCompletor _chatCompletor = chatCompletor;
		private readonly IEmbeddingGenerator _embeddingGenerator = embeddingGenerator;
		private readonly IEmbeddingRepository _repository = repository;

		public async Task RunAsync()
		{
			try
			{
				var chatHistory = new TalonRAGChatHistory(SYSTEM_MESSAGE);

				while (true)
				{
					System.Console.Write("You: ");
					string? userInput = System.Console.ReadLine();

					System.Console.ForegroundColor = ConsoleColor.DarkGreen;

					if (userInput == null || userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
					{
						System.Console.WriteLine("TalonRAG: Goodbye!");
						break;
					}

					var inputEmbedding = await GenerateEmbeddingForInput(userInput);
					var similarArticleEmbeddings = await GetSimilarArticleEmbeddings(inputEmbedding);

					var toolMessage = string.Join(", ", similarArticleEmbeddings.Select(article => article.Content));
					chatHistory.AddToolMessage(toolMessage);
					chatHistory.AddUserMessage(userInput);

					var content = await GenerateChatMessageContent(chatHistory);
					chatHistory.AddAssistantMessage(content ?? "");

					System.Console.WriteLine($"TalonRAG: {content}");

					System.Console.ResetColor();
				}
			}
			catch (Exception ex)
			{
				System.Console.WriteLine($"TalonRAG: Encountered an exception - {ex.Message}");
			}
			finally
			{
				System.Console.ResetColor();
			}
		}

		private async Task<IList<float>> GenerateEmbeddingForInput(string input)
		{
			var embeddings = await _embeddingGenerator.GenerateEmbeddingsAsync([input]);
			return embeddings.FirstOrDefault().ToArray();
		}

		private async Task<IEnumerable<ArticleEmbedding>> GetSimilarArticleEmbeddings(IList<float> embedding)
		{
			return await _repository.GetSimilarEmbeddingsAsync([.. embedding]);
		}

		private async Task<string?> GenerateChatMessageContent(TalonRAGChatHistory chatHistory)
		{
			return await _chatCompletor.GetChatMessageContentAsync(chatHistory);
		}
	}
}
