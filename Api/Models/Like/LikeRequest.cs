namespace Api.Models
{
    public class LikeRequest
    {
        public Guid PostId { get; set; }
        public Guid? UserId { get; set; }
    }
}
