using TalonRAG.Application.DataTransferObjects;
using TalonRAG.Application.Extensions;
using TalonRAG.Application.Interfaces;
using TalonRAG.Domain.Interfaces;

namespace TalonRAG.Application.Services
{
	/// <summary>
	/// API service implementation for retrieving, creating, and updating conversations with TalonRAG.
	/// </summary>
	/// <param name="conversationService">
	/// <see cref="IConversationService"/>.
	/// </param>
    public class ConversationApiService(IConversationService conversationService) : IConversationApiService
	{
		private readonly IConversationService _conversationService = conversationService;

		/// <inheritdoc cref="IConversationApiService.GetConversationByIdAsync(int)" />
		public async Task<ConversationDto?> GetConversationByIdAsync(int id)
		{
			var conversation = await _conversationService.GetConversationByIdAsync(id);
			if (conversation == null) { return null; }

			return conversation.ToDto();
		}

		/// <inheritdoc cref="IConversationApiService.AddNewConversationAsync(NewConversationRequest)" />
		public async Task<ConversationDto?> AddNewConversationAsync(NewConversationRequest request)
		{
			var conversation = await _conversationService.StartConversationAsync(request.UserId);
			conversation = await _conversationService.ContinueConversationAsync(conversation?.Id ?? -1, request.Message ?? string.Empty);
			if (conversation == null) { return null; }

			return conversation.ToDto();
		}

		/// <inheritdoc cref="IConversationApiService.UpdateConversationAsync(int, UpdateConversationRequest)" />
		public async Task<ConversationDto?> UpdateConversationAsync(int conversationId, UpdateConversationRequest request)
		{
			var conversation = await _conversationService.ContinueConversationAsync(conversationId, request.Message ?? string.Empty);
			if (conversation == null) { return null; }

			return conversation.ToDto();
		}
	}
}
