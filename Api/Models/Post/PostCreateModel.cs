namespace Api.Models
{
    public class PostCreateModel
    {
        public string Description { get; set; } = null!;
        public DateTimeOffset DateTimeCreation { get; set; }
        public ICollection<MetadataModel> PostMetas { get; set; } = null!;
    }
}
