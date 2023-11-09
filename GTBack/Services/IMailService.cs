using GTBack.Core.Entities;
using GTBack.Core.Results;

namespace GTBack.Core.Services;

public interface IMailService
{
    Task SendMail(MailData mail);

}