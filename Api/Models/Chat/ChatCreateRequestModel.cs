using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class ChatCreateRequestModel
    {
        public string? Name { get; set; }
        public Guid CreatorId { get; set; }
    }

    public class ChatAddUserRequestModel
    {
        [Required]
        public Guid ChatId { get; set; }
        public Guid InviterId { get; set; }
        [Required]
        public Guid UserId { get; set; }
    }

    public class MessageRequestModel
    {
        [Required]
        public Guid ChatId { get; set; }
        public Guid SenderId { get; set; }
        public string Text { get; set; } = null!;
        public DateTime? SendingTime { get; set; }
        public List<MetadataModel>? Attachments { get; set; }
    }

    public class MessageCreateModel
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public string Text { get; set; } = null!;
        public DateTime SendingTime { get; set; }
        public List<MetadataLinkModel>? Attachments { get; set; }
    }
}
