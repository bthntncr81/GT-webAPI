﻿using GTBack.Core.DTO.Request;
using GTBack.Core.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTBack.Service.Utilities.Jwt
{
    public interface IJwtTokenService
    {
        public AccessTokenDto GenerateAccessToken(CustomerDto userDto);
        public string GenerateRefreshToken();
        public bool Validate(string refreshToken);
    }
}
