namespace Api.Models
{
    public class AttachExternalModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public string? ContentLink { get; set; } = null!;

    }

    public class AttachSimpleExternalModel
    {
        public Guid Id { get; set; }
        public string? ContentLink { get; set; } = null!;

    }
}
