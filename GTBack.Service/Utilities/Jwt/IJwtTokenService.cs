using GTBack.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.Entities;

namespace GTBack.Service.Utilities.Jwt
{
    public interface IJwtTokenService<T> where T : BaseRegisterDTO
    {
        public AccessTokenDto GenerateAccessToken(BaseRegisterDTO userDto);

        AccessTokenDto GenerateAccessTokenCoach(BaseRegisterDTO userDto);
        AccessTokenDto GenerateAccessTokenStudent(BaseRegisterDTO userDto, bool havePermission);

        AccessTokenDto GenerateAccessTokenParent(BaseRegisterDTO userDto, string userType, string logNumber);

        public string GenerateRefreshToken();
        public bool Validate(string refreshToken);
    }
}
