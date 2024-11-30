using TalonRAG.Domain.Entities;
using TalonRAG.Domain.Enums;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Domain.Models;

namespace TalonRAG.Domain.Services
{
	/// <summary>
	/// Conversation manager service class implementation of <see cref="IConversationManagerService" />.
	/// </summary>
	/// <param name="conversationRepository">
	/// <see cref="IConversationRepository" />.
	/// </param>
	/// <param name="messageRepository">
	/// <see cref="IMessageRepository" />.
	/// </param>
	/// <param name="articleEmbeddingService">
	/// <see cref="IArticleEmbeddingService" />.
	/// </param>
	/// <param name="chatCompletionService">
	/// <see cref="IChatCompletionService" />.
	/// </param>
	public class ConversationManagerService(
		IConversationRepository conversationRepository, 
		IMessageRepository messageRepository, 
		IArticleEmbeddingService articleEmbeddingService, 
		IChatCompletionService chatCompletionService) : IConversationManagerService
	{
		private const string SYSTEM_MESSAGE_CONTENT = $@"
			You're an AI assistant called TalonRAG who retrieves the latest news article descriptions for fans of the NFL team, Philadelphia Eagles.
			Based on the descriptions of latest news articles you retrieve, you formulate an informative response based only on the descriptions of latest news articles.
			Don't report anything as fact unless you retrieve it from an article description provided to you as a tool message.
			Don't help the user with anything other than latest news article descriptions on the Philadelphia Eagles.";

		private readonly IConversationRepository _conversationRepository = conversationRepository;
		private readonly IMessageRepository _messageRepository = messageRepository;
		private readonly IArticleEmbeddingService _articleEmbeddingService = articleEmbeddingService;
		private readonly IChatCompletionService _chatCompletionService = chatCompletionService;

		/// <inheritdoc cref="IConversationManagerService.StartConversationAsync(int)" />
		public async Task<int> StartConversationAsync(int userId)
		{
			var conversationId = await _conversationRepository.InsertConversationAsync(
				new ConversationRecord { UserId = userId });

			await _messageRepository.InsertMessageAsync(
				new MessageRecord { ConversationId = conversationId, MessageAuthorRole = MessageAuthorRole.System, Content = SYSTEM_MESSAGE_CONTENT });

			return conversationId;
		}

		/// <inheritdoc cref="IConversationManagerService.ContinueConversationAsync(int, string)" />
		public async Task<string> ContinueConversationAsync(int conversationId, string userMessageContent)
		{
			var conversationRecord =
				await _conversationRepository.GetConversationByIdAsync(conversationId) ?? throw new Exception("Conversation record could not be found, could not continue.");

			var articleEmbeddings = await _articleEmbeddingService.GetSimilarArticleEmbeddingsForMessageContentAsync(userMessageContent);
			var toolMessageContent = string.Join(", ", articleEmbeddings.Select(embedding => embedding.ArticleEmbeddingRecord.Content));

			await _messageRepository.InsertMessageAsync(
				new MessageRecord { ConversationId = conversationId, MessageAuthorRole = MessageAuthorRole.Tool, Content = toolMessageContent });
			await _messageRepository.InsertMessageAsync(
				new MessageRecord { ConversationId = conversationId, MessageAuthorRole = MessageAuthorRole.User, Content = userMessageContent });
			var messageRecords = await _messageRepository.GetMessagesByConversationIdAsync(conversationId);

			var conversation = new Conversation { ConversationRecord = conversationRecord, MessageRecords = messageRecords };

			var assistantMessageContent = await _chatCompletionService.GetChatMessageContentAsync(conversation);
			await _messageRepository.InsertMessageAsync(
				new MessageRecord { ConversationId = conversationId, MessageAuthorRole = MessageAuthorRole.Assistant, Content = assistantMessageContent });

			return assistantMessageContent;
		}
	}
}
