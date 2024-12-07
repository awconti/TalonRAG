using TalonRAG.Domain.Entities;
using TalonRAG.Domain.Enums;
using TalonRAG.Domain.Interfaces;
using TalonRAG.Domain.Models;

namespace TalonRAG.Domain.Services
{
	/// <summary>
	/// Article conversation service class implementation of <see cref="IConversationService" />.
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
	public class ArticleConversationService(
		IConversationRepository conversationRepository, 
		IMessageRepository messageRepository, 
		IArticleEmbeddingService articleEmbeddingService, 
		IChatCompletionService chatCompletionService) : IConversationService
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

		/// <inheritdoc cref="IConversationService.StartConversationAsync(int)" />
		public async Task<Conversation> StartConversationAsync(int userId)
		{
			// Create conversation.
			var conversationId = await _conversationRepository.InsertConversationAsync(
				new ConversationRecord { UserId = userId });

			// Create intiail system message, add to conversation.
			await _messageRepository.InsertMessageAsync(
				new MessageRecord { ConversationId = conversationId, MessageAuthorRole = MessageAuthorRole.System, Content = SYSTEM_MESSAGE_CONTENT });

			// Retrieve conversation and corresponding message records and return started conversation object.
			var conversationRecord = 
				await _conversationRepository.GetConversationByIdAsync(conversationId) ?? throw new Exception("Conversation record could not be found after creation, could not start."); ;
			var messageRecords = await _messageRepository.GetMessagesByConversationIdAsync(conversationId);

			return new Conversation { ConversationRecord = conversationRecord, MessageRecords = messageRecords };
		}

		/// <inheritdoc cref="IConversationService.ContinueConversationAsync(int, string)" />
		public async Task<Conversation> ContinueConversationAsync(int conversationId, string userMessageContent)
		{
			// Retrieve existing conversation record.
			var conversationRecord =
				await _conversationRepository.GetConversationByIdAsync(conversationId) ?? throw new Exception("Conversation record could not be found, could not continue.");

			// Retrieve similar article embeddings based on user message content.
			var articleEmbeddings = await _articleEmbeddingService.GetSimilarArticleEmbeddingsForMessageContentAsync(userMessageContent);

			// Create comma delimited string of similar article description content (to be used as tool message content).
			var toolMessageContent = string.Join(", ", articleEmbeddings.Select(embedding => embedding.ArticleEmbeddingRecord.Content));

			// Create message records for user and tool messages.
			await _messageRepository.InsertMessageAsync(
				new MessageRecord { ConversationId = conversationId, MessageAuthorRole = MessageAuthorRole.Tool, Content = toolMessageContent });
			await _messageRepository.InsertMessageAsync(
				new MessageRecord { ConversationId = conversationId, MessageAuthorRole = MessageAuthorRole.User, Content = userMessageContent });

			// Retrieve current message records for conversation record.
			var messageRecords = await _messageRepository.GetMessagesByConversationIdAsync(conversationId);

			// Build conversation domain object and provide to chat completion service.
			var conversation = new Conversation { ConversationRecord = conversationRecord, MessageRecords = messageRecords };
			var assistantMessageContent = await _chatCompletionService.GetChatMessageContentAsync(conversation);

			// Add assistant message content to conversation.
			await _messageRepository.InsertMessageAsync(
				new MessageRecord { ConversationId = conversationId, MessageAuthorRole = MessageAuthorRole.Assistant, Content = assistantMessageContent });

			// Retrieve updated collection of message records for domain object.
			messageRecords = await _messageRepository.GetMessagesByConversationIdAsync(conversationId);
			conversation.MessageRecords = messageRecords;

			return conversation;
		}
	}
}
