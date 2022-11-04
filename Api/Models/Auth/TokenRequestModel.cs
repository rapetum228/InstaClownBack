namespace Api.Models.Auth
{
    public class TokenRequestModel
    {
        public string Login { get; set; } = null!;
        public string Pass { get; set; } = null!;
    }
}
