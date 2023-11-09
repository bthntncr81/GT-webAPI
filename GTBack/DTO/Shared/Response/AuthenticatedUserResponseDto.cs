

namespace GTBack.Core.DTO
{
    public class AuthenticatedUserResponseDto
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpirationTime { get; set; }
        public string RefreshToken { get; set; }
        public bool IsTemp { get; set; }
        
    }
}