using GTBack.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTBack.Core.UnitOfWorks;

namespace GTBack.Repository.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>
    {
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenRepository(AppDbContext context) : base(context)
        {
            _context = context;

        }

    }
}
