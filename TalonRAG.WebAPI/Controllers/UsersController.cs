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
		public async Task<ActionResult<IList<ConversationDto>>> GetConversationsByUserIdAsync(int id)
		{
			var response = await _userApiService.GetConversationsByUserIdAsync(id);
			return StatusCode(StatusCodes.Status200OK, response);
		}
	}
}
