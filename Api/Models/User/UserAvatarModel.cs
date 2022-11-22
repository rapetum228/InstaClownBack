namespace Api.Models
{
    public interface IAvatar
    {
        string? AvatarLink { get; set; }
    }
    public class UserAvatarModel : UserModel, IAvatar
    {
        public string? AvatarLink { get; set; }
       
    }

    public class UserSimpleModel: IAvatar
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? AvatarLink { get; set; }
    }
}
