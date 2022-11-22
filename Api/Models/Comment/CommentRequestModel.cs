

namespace Api.Models
{
    public class CommentRequestModel
    {
        public Guid PostId { get; set; }
        public Guid? AuthorId { get; set; }
        public string Text { get; set; } = null!;
        public DateTimeOffset? DateTimeWriting { get; set; }
    }

    public class CommentToCommentRequestModel
    {
        public Guid CommentId { get; set; }
        public Guid? AuthorId { get; set; }
        public string Text { get; set; } = null!;
        public DateTimeOffset? DateTimeWriting { get; set; }
    }

    public class CommentModel
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public UserSimpleModel User { get; set; } = null!;
        public string Text { get; set; } = null!;
        public Guid? CommentId { get; set; } 
        public DateTimeOffset DateTimeWriting { get; set; }
    }
}
