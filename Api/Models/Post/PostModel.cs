namespace Api.Models
{
    public class PostModel
    {
        public Guid Id { get; set; }
        public UserAvatarModel Author { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTimeOffset DateTimeCreation { get; set; }
        public List<AttachExternalModel>? Contents { get; set; } = new List<AttachExternalModel>();
    }
}
