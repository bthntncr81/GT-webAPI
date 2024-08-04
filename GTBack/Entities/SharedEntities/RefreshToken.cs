using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTBack.Core.Entities.Shopping;

namespace GTBack.Core.Entities
{
    public class RefreshToken 
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public ShoppingUser? User { get; set; }
        public int? customerId { get; set; }
      
    }
}
