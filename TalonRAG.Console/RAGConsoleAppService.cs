﻿using TalonRAG.Domain.Entities;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Infrastructure.SemanticKernel.ChatCompletion;
using TalonRAG.Infrastructure.SemanticKernel.Embedding;

internal class RAGConsoleAppService(
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

                var toolMessage = string.Join(", ", similarArticleEmbeddings.Select(article => article.Content));
                chatHistory.AddToolMessage(toolMessage);
                chatHistory.AddUserMessage(userInput);

                var content = await GenerateChatMessageContent(chatHistory);
                chatHistory.AddAssistantMessage(content ?? "");

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
