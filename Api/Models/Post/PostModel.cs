namespace Api.Models
{
    public class PostModel
    {
        public Guid Id { get; set; }
        public UserSimpleModel Author { get; set; } = null!; //public UserAvatarModel Author { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTimeOffset DateTimeCreation { get; set; }
        public int LikesCount { get; set; }
        public List<AttachExternalModel>? Contents { get; set; } = new List<AttachExternalModel>();
    }

    public class PostSimpleModel
    {
        public Guid Id { get; set; }
        public UserSimpleModel Author { get; set; } = null!; //public UserAvatarModel Author { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTimeOffset DateTimeCreation { get; set; }
        public int LikesCount { get; set; }
        public List<AttachSimpleExternalModel>? Contents { get; set; } = new List<AttachSimpleExternalModel>();
    }
}
