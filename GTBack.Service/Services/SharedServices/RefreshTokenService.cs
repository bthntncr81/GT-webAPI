using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.Entities;
using GTBack.Core.Services;
using GTBack.Core.UnitOfWorks;
using GTBack.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTBack.Service.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly RefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RefreshTokenService(RefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<RefreshToken?> GetByToken(string token)
        {
            return _refreshTokenRepository.Where(x => x.Token == token).Include(x => x.User).FirstOrDefaultAsync();



        }

        public async Task Create(RefreshTokenDto refreshTokenDto)
        {
            var refreshToken = _mapper.Map<RefreshToken>(refreshTokenDto);
           await  _refreshTokenRepository.AddAsync(refreshToken);
        }

        public async Task Delete(Guid id)
        {
            var refreshToken = await _refreshTokenRepository.FindAsync(rt => rt.Id == id);
            _refreshTokenRepository.Remove(refreshToken);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAll(int userId)
        {
         
                    _refreshTokenRepository.Remove(x => x.customerId == userId);
                 
            await _unitOfWork.CommitAsync();
        }

     
    }
}
