using GTBack.Core.DTO;
using GTBack.Core.Entities;
using GTBack.Core.Results;

namespace GTBack.Core.Services;

public interface IMailService
{
    void SendEmail(MailServiceOptions options);

}