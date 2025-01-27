using TalonRAG.Application.DataTransferObjects;
using TalonRAG.Application.DataTransferObjects.Requests;
using TalonRAG.Application.Exceptions;
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
    public class ConversationApiService(IConversationService conversationService, IUserService userService) : IConversationApiService
	{
		private readonly IConversationService _conversationService = conversationService;
		private readonly IUserService _userService = userService;

		/// <inheritdoc cref="IConversationApiService.GetConversationByIdAsync(int)" />
		public async Task<ConversationDto> GetConversationByIdAsync(int id)
		{
			var conversation = await _conversationService.GetConversationByIdAsync(id);
			return conversation is not null ? conversation.ToDto() : throw new ConversationNotFoundException(id);
		}

		/// <inheritdoc cref="IConversationApiService.AddNewCopenonversationAsync(NewConversationRequest)" />
		public async Task<ConversationDto> AddNewConversationAsync(NewConversationRequest request)
		{
			_ = await _userService.GetUserByIdAsync(request.UserId) ?? throw new UserNotFoundException(request.UserId);
			var conversation = await _conversationService.StartConversationAsync(request.UserId);
			conversation = await _conversationService.ContinueConversationAsync(conversation?.Id ?? -1, request.MessageContent ?? string.Empty);
			return conversation is not null ? conversation.ToDto() : throw new ConversationInitializationException(request.UserId);
		}

		/// <inheritdoc cref="IConversationApiService.UpdateConversationAsync(int, UpdateConversationRequest)" />
		public async Task<ConversationDto> UpdateConversationAsync(int conversationId, UpdateConversationRequest request)
		{
			var conversation = await _conversationService.ContinueConversationAsync(conversationId, request.MessageContent ?? string.Empty);
			return conversation is not null ? conversation.ToDto() : throw new ConversationNotFoundException(conversationId);
		}
	}
}
