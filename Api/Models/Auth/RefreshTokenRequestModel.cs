namespace Api.Models.Auth
{
    public class RefreshTokenRequestModel
    {
        public RefreshTokenRequestModel(string refreshToken)
        {
            RefreshToken = refreshToken;
        }

        public string RefreshToken { get; set; }
    }
}
