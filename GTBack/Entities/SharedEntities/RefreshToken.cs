using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTBack.Core.Entities
{
    public class RefreshToken 
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public User? User { get; set; }
        public int? customerId { get; set; }
      
    }
}
