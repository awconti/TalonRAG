using Microsoft.AspNetCore.Mvc;
using TalonRAG.Application.DataTransferObjects;
using TalonRAG.Application.Interfaces;

namespace TalonRAG.WebAPI.Controllers
{
	[Route("api/users")]
	[ApiController]
	public class UsersController(IUserApiService userApiService) : ControllerBase
	{
		private readonly IUserApiService _userApiService = userApiService;

		[HttpGet("{id}/conversations")]
		public async Task<ActionResult<IList<ConversationDto>>> GetConversationsByUserIdAsync(int id, bool lastMessagesOnly)
		{
			var response =
				!lastMessagesOnly
					? await _userApiService.GetConversationsByUserIdAsync(id)
					: await _userApiService.GetLastMessagesInConversationsByUserIdAsync(id);
			return StatusCode(StatusCodes.Status200OK, response);
		}
	}
}
