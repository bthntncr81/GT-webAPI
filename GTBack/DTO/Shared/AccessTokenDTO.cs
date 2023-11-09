using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTBack.Core.DTO
{
    public class AccessTokenDto
    {
        public string Value { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}