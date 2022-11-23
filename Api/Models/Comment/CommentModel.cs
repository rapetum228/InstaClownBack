

namespace Api.Models
{
    public class CommentModel
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public UserSimpleModel User { get; set; } = null!;
        public string Text { get; set; } = null!;
        public Guid? ResponseCommentId { get; set; } 
        public DateTimeOffset DateTimeWriting { get; set; }
    }

    public class CommentSimpleModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
}
