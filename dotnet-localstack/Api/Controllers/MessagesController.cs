using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
	private readonly ISqsService _sqsService;

	public MessagesController(ISqsService sqsService)
	{
		_sqsService = sqsService;
	}

	[HttpPost]
	public async Task<IActionResult> SendMessage([FromBody] string message)
	{
		var messageId = await _sqsService.SendMessageAsync(message);
		return Ok(new { MessageId = messageId });
	}

	[HttpGet]
	public async Task<IActionResult> ReceiveMessages([FromQuery] int maxMessages = 10)
	{
		var messages = await _sqsService.ReceiveMessagesAsync(maxMessages);
		return Ok(messages);
	}

	[HttpDelete("{receiptHandle}")]
	public async Task<IActionResult> DeleteMessage(string receiptHandle)
	{
		await _sqsService.DeleteMessageAsync(receiptHandle);
		return Ok();
	}
}
