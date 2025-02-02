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

		/// <inheritdoc cref="IConversationService.GetConversationByIdAsync(int)"
		public async Task<ConversationModel?> GetConversationByIdAsync(int conversationId) => await GetExistingConversationByIdAsync(conversationId);

		/// <inheritdoc cref="IConversationService.GetConversationsByUserIdAsync(int)" />
		public async Task<IList<ConversationModel>?> GetConversationsByUserIdAsync(int userId) => await GetExistingConversationsByUserIdAsync(userId);

		/// <inheritdoc cref="IConversationService.GetLastMessagesInConversationsByUserIdAsync(int)" />
		public async Task<IList<ConversationModel>?> GetLastMessagesInConversationsByUserIdAsync(int userId) => await GetExistingConversationsByUserIdAsync(userId, true);

		/// <inheritdoc cref="IConversationService.StartConversationAsync(int)" />
		public async Task<ConversationModel?> StartConversationAsync(int userId)
		{
			// Create conversation.
			var conversationId = await _conversationRepository.InsertConversationAsync(userId);

			// Create initial system message, add to conversation.
			var systemMessage = 
				new MessageModel { ConversationId = conversationId, MessageType = MessageType.System, Content = SYSTEM_MESSAGE_CONTENT };
			await _messageRepository.InsertMessageAsync(systemMessage);

			// Retrieve conversation and corresponding message records and return started conversation object.
			return await GetExistingConversationByIdAsync(conversationId);
		}

		/// <inheritdoc cref="IConversationService.ContinueConversationAsync(int, string)" />
		public async Task<ConversationModel?> ContinueConversationAsync(int conversationId, string userMessageContent)
		{
			// Get existing conversation by ID.
			var conversation = await GetConversationByIdAsync(conversationId);
			if (conversation is null) { return null; }

			// Retrieve similar article embeddings based on user message content.
			var articleEmbeddings = await _articleEmbeddingService.GetSimilarEmbeddingsFromContentAsync(userMessageContent);

			// Create comma delimited string of similar article description content (to be used as tool message content).
			var toolMessageContent = string.Join(", ", articleEmbeddings.Select(embedding => embedding.Content));

			// Create message records of type user and tool and add to conversation.
			var userMessage =
				new MessageModel { ConversationId = conversationId, MessageType = MessageType.User, Content = userMessageContent };
			var toolMessage =
				new MessageModel { ConversationId = conversationId, MessageType = MessageType.Tool, Content = toolMessageContent };

			await _messageRepository.InsertMessageAsync(toolMessage);
			await _messageRepository.InsertMessageAsync(userMessage);

			conversation.AddMessages([toolMessage, userMessage]);

			// Generate assistant message content and add to conversation.
			var assistantMessageContent = await _chatCompletionService.GetChatMessageContentAsync(conversation);
			var assistantMessage =
				new MessageModel { ConversationId = conversationId, MessageType = MessageType.Assistant, Content = assistantMessageContent };
			
			await _messageRepository.InsertMessageAsync(assistantMessage);

			conversation.AddMessages([assistantMessage]);

			return conversation;
		}

		/// <inheritdoc cref="IConversationService.DeleteConversationByIdAsync(int)" />
		public async Task DeleteConversationByIdAsync(int conversationId)
		{
			// Get existing conversation by ID.
			var conversation = await GetConversationByIdAsync(conversationId);
			if (conversation is null) { return; }

			await _messageRepository.DeleteMessagesByConversationIdAsync(conversationId);
			await _conversationRepository.DeleteConversationByIdAsync(conversationId);
		}

		private async Task<ConversationModel?> GetExistingConversationByIdAsync(int conversationId)
		{
			var conversation =
				await _conversationRepository.GetConversationByIdAsync(conversationId);
			if (conversation is null) { return null; }

			var messages = await _messageRepository.GetMessagesByConversationIdAsync(conversation.Id);
			conversation.AddMessages(messages);

			return conversation;
		}

		private async Task<IList<ConversationModel>?> GetExistingConversationsByUserIdAsync(int userId, bool getLastMessages = false)
		{
			var conversations = await _conversationRepository.GetConversationsByUserIdAsync(userId);
			if (conversations is null) { return null; }

			var conversationIds = conversations.Select(conversation => conversation.Id).ToArray();

			var messages =
				getLastMessages is true
					? await _messageRepository.GetLastMessagesByConversationIdsAsync(conversationIds)
					: await _messageRepository.GetMessagesByConversationIdsAsync(conversationIds);
			
			var messagesByConversationIds =
					messages.GroupBy(message => message.ConversationId)
						.ToDictionary(group => group.Key, group => group.ToList());

			foreach (var conversation in conversations)
			{
				if (messagesByConversationIds.TryGetValue(conversation.Id, out var scopedMessages))
				{
					conversation.AddMessages(scopedMessages);
				}
			}

			return conversations;
		}
	}
}
