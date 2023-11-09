
using GTBack.Core.DTO;
using GTBack.Core.Entities;
using GTBack.Core.Repositories;
using GTBack.Core.Services;
using GTBack.Core.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GTBack.Service.Services
{

    public class Service<T>: IService<T> where T : class
    {

        private readonly IGenericRepository<T> _repository;

    
        private readonly IUnitOfWork _unitOfWork;

        public Service(IUnitOfWork unitOfWork, IGenericRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<T> AddAsync(T entity)
        { 
            await  _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
           
        }
        
        

        
        
        public Task SendMail(MailData mail)
        {
            var client = new SmtpClient("smtp-mail.outlook.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential("batuhanntuncerr@hotmail.com", "170717doga")
            };
            MailMessage message = new MailMessage(mail.SenderMail, mail.RecieverMail, mail.EmailSubject, mail.EmailBody);

            message.IsBodyHtml = true;
            return client.SendMailAsync(message);
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            return entities;
        }

        public async Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
return await _repository.AnyAsync(expression); 
                
                }

        public  async Task RemoveAsync(T entity)
        {

        _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAll().ToListAsync();



                }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> expression)
        {
            return await _repository.GetByIdAsync(expression);
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {

            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();

        }

        public async Task UpdateAsync(T entity)
        {
             _repository.Update(entity);
            await _unitOfWork.CommitAsync();    
           
        }

        public IQueryable<T> Where(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {

            return _repository.Where(expression);
        }

        public Task<T?> FindAsNoTrackingAsync(Expression<Func<T, bool>> expression)
        {
            return _repository.FindAsNoTrackingAsync(expression);
        }


        public async Task<T?> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await _repository.FindAsync(expression);
                
                
                }

        public void Remove(Expression<Func<T, bool>> expression)
        {
              _repository.Remove(expression);
        }
        
     

    }
}
