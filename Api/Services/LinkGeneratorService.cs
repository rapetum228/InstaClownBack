using Api.Models;
using DAL.Entities;

namespace Api.Services
{
    public class LinkGeneratorService
    {
        public Func<PostAttach, string?>? LinkContentGenerator;
        public Func<User, string?>? LinkAvatarGenerator;

        public void FixAvatar(User s, IAvatar d)
        {
            d.AvatarLink = s.Avatar == null ? null : LinkAvatarGenerator?.Invoke(s);
        }
        public void FixContent(PostAttach s, AttachExternalModel d)
        {
            d.ContentLink = LinkContentGenerator?.Invoke(s);
        }

    }
}
