using TalonRAG.Domain.Entities;
using TalonRAG.Domain.Enums;
using TalonRAG.Domain.Extensions;
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
	/// <see cref="IEmbeddingService" />.
	/// </param>
	/// <param name="chatCompletionService">
	/// <see cref="IChatCompletionService" />.
	/// </param>
	public class ArticleConversationService(
		IConversationRepository conversationRepository, 
		IMessageRepository messageRepository, 
		IEmbeddingService articleEmbeddingService, 
		IChatCompletionService chatCompletionService) : IConversationService
	{
		private const string SYSTEM_MESSAGE_CONTENT = $@"
			You're an AI assistant called TalonRAG who retrieves the latest news article descriptions for fans of the NFL team, Philadelphia Eagles.
			Based on the descriptions of latest news articles you retrieve, you formulate an informative response based only on the descriptions of latest news articles.
			Don't report anything as fact unless you retrieve it from an article description provided to you as a tool message.
			Don't help the user with anything other than latest news article descriptions on the Philadelphia Eagles.";

		private readonly IConversationRepository _conversationRepository = conversationRepository;
		private readonly IMessageRepository _messageRepository = messageRepository;
		private readonly IEmbeddingService _articleEmbeddingService = articleEmbeddingService;
		private readonly IChatCompletionService _chatCompletionService = chatCompletionService;

		/// <inheritdoc cref="IConversationService.GetConversationByIdAsync(int)"
		public async Task<Conversation?> GetConversationByIdAsync(int id) => await GetExistingConversationByIdAsync(id);

		/// <inheritdoc cref="IConversationService.StartConversationAsync(int)" />
		public async Task<Conversation?> StartConversationAsync(int userId)
		{
			// Create conversation.
			var conversationRecord = new ConversationRecord { UserId = userId };
			var conversationId = await _conversationRepository.InsertConversationAsync(conversationRecord);

			// Create initial system message, add to conversation.
			var systemMessageRecord = 
				new MessageRecord { ConversationId = conversationId, MessageAuthorRole = MessageAuthorRole.System, Content = SYSTEM_MESSAGE_CONTENT };
			await _messageRepository.InsertMessageAsync(systemMessageRecord);

			// Retrieve conversation and corresponding message records and return started conversation object.
			return await GetExistingConversationByIdAsync(conversationId);
		}

		/// <inheritdoc cref="IConversationService.ContinueConversationAsync(int, string)" />
		public async Task<Conversation?> ContinueConversationAsync(int conversationId, string userMessageContent)
		{
			// Get existing conversation by ID.
			var conversation = await GetConversationByIdAsync(conversationId);
			if (conversation == null) { return null; }

			// Retrieve similar article embeddings based on user message content.
			var articleEmbeddings = await _articleEmbeddingService.GetSimilarEmbeddingsFromContentAsync(userMessageContent);

			// Create comma delimited string of similar article description content (to be used as tool message content).
			var toolMessageContent = string.Join(", ", articleEmbeddings.Select(embedding => embedding.Content));

			// Create message records for user and tool messages and add to conversation.
			var userMessageRecord =
				new MessageRecord { ConversationId = conversationId, MessageAuthorRole = MessageAuthorRole.User, Content = userMessageContent };
			var toolMessageRecord =
				new MessageRecord { ConversationId = conversationId, MessageAuthorRole = MessageAuthorRole.Tool, Content = toolMessageContent };

			await _messageRepository.InsertMessageAsync(toolMessageRecord);
			await _messageRepository.InsertMessageAsync(userMessageRecord);

			conversation.AddMessages(
				[toolMessageRecord.ToDomainModel(), userMessageRecord.ToDomainModel()]);

			// Generate assistant message content and add to conversation.
			var assistantMessageContent = await _chatCompletionService.GetChatMessageContentAsync(conversation);
			var assistantMessageRecord =
				new MessageRecord { ConversationId = conversationId, MessageAuthorRole = MessageAuthorRole.Assistant, Content = assistantMessageContent };
			
			await _messageRepository.InsertMessageAsync(assistantMessageRecord);

			conversation.AddMessages([assistantMessageRecord.ToDomainModel()]);

			return conversation;
		}

		private async Task<Conversation?> GetExistingConversationByIdAsync(int id)
		{
			var conversationRecord =
				await _conversationRepository.GetConversationByIdAsync(id);
			if(conversationRecord == null) { return null; }

			var messageRecords = await _messageRepository.GetMessagesByConversationIdAsync(conversationRecord.Id);

			return conversationRecord.ToDomainModel(messageRecords);
		}
	}
}
