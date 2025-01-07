using Microsoft.AspNetCore.Mvc;
using TalonRAG.Application.DataTransferObjects;
using TalonRAG.Application.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TalonRAG.WebAPI.Controllers
{
	[Route("api/conversations")]
	[ApiController]
	public class ConversationsController(IConversationApiService conversationApiService) : ControllerBase
	{
		private readonly IConversationApiService _conversationApiService = conversationApiService;

		[HttpGet("{id}")]
		public async Task<ActionResult<ConversationDto>> GetConversationByIdAsync(int id)
		{
			var response = await _conversationApiService.GetConversationByIdAsync(id);
			if (response == null)
			{
				return StatusCode(StatusCodes.Status404NotFound);
			}

			return StatusCode(StatusCodes.Status200OK, response);
		}

		[HttpPost]
		public async Task<ActionResult<ConversationDto>> AddNewConversationAsync(NewConversationRequest request)
		{
			var response = await _conversationApiService.AddNewConversationAsync(request);
			if (response == null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}

			return StatusCode(StatusCodes.Status201Created, response);
		}

		[HttpPut("{id}/messages")]
		public async Task<ActionResult<ConversationDto>> UpdateConversationAsync(int id, UpdateConversationRequest request)
		{
			var response = await _conversationApiService.UpdateConversationAsync(id, request);
			if (response == null)
			{
				return StatusCode(StatusCodes.Status404NotFound);
			}

			return StatusCode(StatusCodes.Status202Accepted, response);
		}
	}
}
