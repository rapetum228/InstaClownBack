using Api.Exceptions;
using Api.Models;
using AutoMapper;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services
{
    public class ChatService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public ChatService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task CreateChat(ChatCreateRequestModel chatRequest)
        {
            var creator = await GetUser(chatRequest.CreatorId);
            //var chat = new Chat
            //{
            //    Id = Guid.NewGuid(),
            //    Name = chatRequest.Name,
            //    Participants = new List<User> { creator }
            //};
            var chat = _mapper.Map<Chat>(chatRequest);
            chat.Participants = new List<User>();
            chat.Participants.Add(creator);
            await _context.Chats.AddAsync(chat);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserInChat(ChatAddUserRequestModel chatAddRequest)
        {
            var user = await GetUser(chatAddRequest.UserId);
            var inviter = await GetUser(chatAddRequest.InviterId);
            var chat = await _context.Chats.Include(c => c.Participants).FirstOrDefaultAsync(c => c.Id == chatAddRequest.ChatId);
            if (chat == default)
            {
                throw new NotFoundException("Chat not found");
            }
            if (!chat.Participants.Contains(inviter))
            {
                throw new NoAccessException("You are not in this chat");
            }
            if (chat.Participants.Contains(user))
            {
                throw new Exception("The user is already a member of this chat");
            }

            chat.Participants.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task SendMesaageInChat(MessageRequestModel messageRequest)
        {
            var sender = await GetUser(messageRequest.SenderId);
            var chat = await _context.Chats.Include(c => c.Participants)
                                           .Include(c => c.Messages)
                                           .FirstOrDefaultAsync(c => c.Id == messageRequest.ChatId);
            if (chat == default)
            {
                throw new NotFoundException("Chat not found");
            }
            if (chat!.Participants.Contains(sender))
            {
                throw new NoAccessException("You can't write to this chat");
            }
            var messageModel = _mapper.Map<MessageCreateModel>(messageRequest);
            if (messageModel.Attachments != null)
            {
                messageModel.Attachments.ForEach(x =>
                {
                    MoveAttach(x, messageModel.SenderId);
                });
            }
            var dbModel = _mapper.Map<Message>(messageModel);
            await _context.Messages.AddAsync(dbModel);
            await _context.SaveChangesAsync();
        }

        private async Task<User> GetUser(Guid userId)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
            if (user == default)
            {
                throw new NotFoundException("User not found");
            }
            return user;
        }

        private void MoveAttach(MetadataLinkModel model, Guid authorId)
        {
            model.AuthorId = authorId;
            model.FilePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "attaches",
               model.TempId.ToString());

            var tempFi = new FileInfo(Path.Combine(Path.GetTempPath(), model.TempId.ToString()));
            if (tempFi.Exists)
            {
                var destFi = new FileInfo(model.FilePath);
                if (destFi.Directory != null && !destFi.Directory.Exists)
                    destFi.Directory.Create();

                File.Move(tempFi.FullName, model.FilePath, true);
            }
        }
    }
}
