

namespace Api.Models
{
    public class CommentRequestModel
    {
        public Guid PostId { get; set; }
        public Guid? AuthorId { get; set; }
        public string Text { get; set; } = null!;
        public DateTimeOffset? DateTimeWriting { get; set; }
    }

}
