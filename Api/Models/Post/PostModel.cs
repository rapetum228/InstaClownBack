namespace Api.Models
{
    public class PostModel
    {
        public string Description { get; set; } = null!;
        public DateTimeOffset DateTimeCreation { get; set; }
        public ICollection<PostAttachOutputModel> PostMetas { get; set; } = null!;
    }
}
