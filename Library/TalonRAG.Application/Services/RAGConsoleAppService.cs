﻿using TalonRAG.Domain.Entities;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Domain.Models;

namespace TalonRAG.Application.Services
{
    /// <summary>
    /// RAG console application service class implementation of <see cref="IConsoleAppService" />.
    /// </summary>
    /// <param name="chatCompletor">
    /// <see cref="IChatCompletor" />.
    /// </param>
    /// <param name="embeddingGenerator">
    /// <see cref="IEmbeddingGenerator" />.
    /// </param>
    /// <param name="repository">
    /// <see cref="IArticleEmbeddingRepository" />.
    public class RAGConsoleAppService(
		IChatCompletor chatCompletor, IEmbeddingGenerator embeddingGenerator, IArticleEmbeddingRepository repository) : IConsoleAppService
	{
		private const string SYSTEM_MESSAGE = $@"
			You're an AI assistant called TalonRAG who retrieves the latest news article descriptions for fans of the NFL team, Philadelphia Eagles.
			Based on the descriptions of latest news articles you retrieve, you formulate an informative response based only on the descriptions of latest news articles.
			Don't report anything as fact unless you retrieve it from an article description provided to you as a tool message.
			Don't help the user with anything other than latest news article descriptions on the Philadelphia Eagles.";

		private readonly IChatCompletor _chatCompletor = chatCompletor;
		private readonly IEmbeddingGenerator _embeddingGenerator = embeddingGenerator;
		private readonly IArticleEmbeddingRepository _repository = repository;

		/// <inheritdoc cref="IConsoleAppService" />
		public async Task RunAsync()
		{
			try
			{
				var conversation = new Conversation();
				conversation.AddSystemMessage(SYSTEM_MESSAGE);

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

					var inputEmbedding = await GenerateEmbeddingForInputAsync(userInput);
					var similarArticleEmbeddings = await GetSimilarArticleEmbeddingsAsync(inputEmbedding);

					var toolMessage = string.Join(", ", similarArticleEmbeddings.Select(article => article.Content));
					conversation.AddToolMessage(toolMessage);
					conversation.AddUserMessage(userInput);

					var content = await GenerateChatMessageContentAsync(conversation);
					conversation.AddAssistantMessage(content ?? "");

					Console.WriteLine($"TalonRAG: {content}");

					Console.ResetColor();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"TalonRAG: Encountered an exception - {ex.Message}");
			}
			finally
			{
				Console.ResetColor();
			}
		}

		private async Task<IList<float>> GenerateEmbeddingForInputAsync(string input)
		{
			var embeddings = await _embeddingGenerator.GenerateEmbeddingsAsync([input]);
			return embeddings.FirstOrDefault().ToArray();
		}

		private async Task<IEnumerable<ArticleEmbeddingRecord>> GetSimilarArticleEmbeddingsAsync(IList<float> embedding)
		{
			return await _repository.GetSimilarEmbeddingsAsync([.. embedding]);
		}

		private async Task<string?> GenerateChatMessageContentAsync(Conversation chatHistory)
		{
			return await _chatCompletor.GetChatMessageContentAsync(chatHistory);
		}
	}
}