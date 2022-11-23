

namespace Api.Models
{
    public class CommentToCommentRequestModel
    {
        public Guid CommentId { get; set; }
        public Guid? AuthorId { get; set; }
        public string Text { get; set; } = null!;
        public DateTimeOffset? DateTimeWriting { get; set; }
    }
}
