using Api.Exceptions;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;

        public ChatController(ChatService chatService, LinkGeneratorService links)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task CreateChat(ChatCreateRequestModel chatRequest)
        {
            if (chatRequest.CreatorId == default)
            {
                chatRequest.CreatorId = GetCurrentUserId();
            }
            await _chatService.CreateChat(chatRequest);
        }

        [HttpPost]
        public async Task AddUserInChat(ChatAddUserRequestModel chatAddRequest)
        {
            if (chatAddRequest.InviterId == default)
            {
                chatAddRequest.InviterId = GetCurrentUserId();
            }
            await _chatService.AddUserInChat(chatAddRequest);
        }

        [HttpPost]
        public async Task SendMesaageInChat(MessageRequestModel messageRequest)
        {
            if (messageRequest.SenderId == default)
            {
                messageRequest.SenderId = GetCurrentUserId();
            }
            await _chatService.SendMesaageInChat(messageRequest);
        }

        private Guid GetCurrentUserId()
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (Guid.TryParse(userIdString, out var userId))
            {
                return userId;
            }
            else
                throw new TypeMismatchException("The entered Id does not match Guid!");
        }
    }
}
