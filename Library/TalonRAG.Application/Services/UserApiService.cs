using TalonRAG.Application.DataTransferObjects;
using TalonRAG.Application.Exceptions;
using TalonRAG.Application.Extensions;
using TalonRAG.Application.Interfaces;
using TalonRAG.Domain.Interfaces;

namespace TalonRAG.Application.Services
{
	/// <summary>
	/// API service class implementation for retrieving, creating, and updating users of the TalonRAG solution. 
	/// </summary>
	/// <param name="userService">
	/// <see cref="IUserService"/>.
	/// </param>
	/// <param name="conversationService">
	/// <see cref="IConversationService"/>.
	/// </param>
	public class UserApiService(IUserService userService, IConversationService conversationService) : IUserApiService
	{
		private readonly IUserService _userService = userService;
		private readonly IConversationService _conversationService = conversationService;

		/// <inheritdoc cref="IUserApiService.GetConversationsByUserIdAsync(int)" />
		public async Task<IList<ConversationDto>> GetConversationsByUserIdAsync(int userId)
		{
			var user = await _userService.GetUserByIdAsync(userId) ?? throw new UserNotFoundApiException(userId);
			var conversations = await _conversationService.GetConversationsByUserIdAsync(userId);
			return conversations is not null
				? conversations.Select(conversation => conversation.ToDto()).ToList() 
				: throw new UserConversationsNotFoundApiException(userId);
		}

		/// <inheritdoc cref="IUserApiService.GetLastMessagesInConversationsByUserIdAsync(int)" />
		public async Task<IList<ConversationDto>> GetLastMessagesInConversationsByUserIdAsync(int userId)
		{
			var user = await _userService.GetUserByIdAsync(userId) ?? throw new UserNotFoundApiException(userId);
			var conversations = await _conversationService.GetLastMessagesInConversationsByUserIdAsync(userId);
			return conversations is not null
				? conversations.Select(conversation => conversation.ToDto()).ToList()
				: throw new UserConversationsNotFoundApiException(userId);
		}
	}
}
