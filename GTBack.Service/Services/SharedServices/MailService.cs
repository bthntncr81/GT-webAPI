using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using GTBack.Core.Entities;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Service.Utilities.Jwt;
using Microsoft.AspNetCore.Http;

namespace GTBack.Service.Services;

public class MailService:IMailService
{
    public Task  SendMail(MailData mail)
    {


        var client = new SmtpClient("smtp-mail.outlook.com", 587)
        {
            EnableSsl =true,
            Credentials = new NetworkCredential("drleventtuncerklinik@hotmail.com","Bthntncr81.")
        };
      
        return     client.SendMailAsync(new MailMessage(
            from: mail.SenderMail,
            to: mail.RecieverMail,
             mail.EmailSubject, mail.EmailBody
        ));
        
    }
}