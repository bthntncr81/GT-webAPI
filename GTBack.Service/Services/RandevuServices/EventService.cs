using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using GTBack.Core.DTO;
using GTBack.Core.Entities;
using GTBack.Core.Repositories;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.UnitOfWorks;
using GTBack.Service.Utilities.Jwt;
using GTBack.Service.Validation;
using GTBack.Service.Validation.Tool;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GTBack.Service.Services;

public class EventService : IEventService
{
    private readonly IGenericRepository<Event> _eventRepository;
    private readonly IGenericRepository<EventType> _eventTypeRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<EventTypeCompanyRelation> _eventTypeCompanyRelationRepository;
    private readonly IGenericRepository<Company> _companyRepository;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ClaimsPrincipal? _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidatorFactory _validatorFactory;
    private readonly IJwtTokenService<BaseRegisterDTO> _tokenService;

    public EventService(IGenericRepository<Event> eventRepository,
        IGenericRepository<EventType> eventTypeRepository,
        IGenericRepository<User> userRepository,
        IGenericRepository<EventTypeCompanyRelation> eventTypeCompanyRelationRepository,
        IGenericRepository<Company> companyRepository,
        IRefreshTokenService refreshTokenService, IJwtTokenService<BaseRegisterDTO> tokenService,
        IValidatorFactory validatorFactory, IHttpContextAccessor httpContextAccessor, IService<Event> service,
        IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _eventRepository = eventRepository;
        _userRepository = userRepository;
        _eventTypeRepository = eventTypeRepository;
        _companyRepository = companyRepository;
        _eventTypeCompanyRelationRepository = eventTypeCompanyRelationRepository;
        _loggedUser = httpContextAccessor.HttpContext?.User;
        _validatorFactory = validatorFactory;
        _refreshTokenService = refreshTokenService;
        _tokenService = tokenService;
    }


    public async Task<IResults> CreateEvent(EventAddRequestDTO model)
    {
        var valResult =
            FluentValidationTool.ValidateModelWithKeyResult<EventAddRequestDTO>(new EventCreateValidator(), model);
        if (valResult.Success == false)
        {
            return new ErrorDataResults<AuthenticatedUserResponseDto>(HttpStatusCode.BadRequest, valResult.Errors);
        }

        var user = _userRepository.Where(x => x.Id == model.ClientUserId).FirstOrDefault();
        var eventModel = _mapper.Map<Event>(model);
        model.StatusId = 0;
        await _eventRepository.AddAsync(eventModel);
        string mailBody = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n\n<head>\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge,chrome=1\">\n    <title>DR Levent Tuncer</title>\n\n</head>\n<style>\n    #button {\n        display: flex;\n        align-items: center;\n        justify-content: center;\n        text-decoration: none;\n    }\n    #button-wrapper {\n        display: flex;\n        align-items: center;\n        justify-content: center;\n    }\n    #title{\n        text-align: center;\n    }\n    .buttonContent {\n      color: #FFFFFF;\n      font-family: Helvetica;\n      font-size: 18px;\n      font-weight: bold;\n      line-height: 100%;\n      padding: 15px;\n      text-align: center;\n    }\n\n    .buttonContent a {\n      color: #FFFFFF;\n      display: block;\n      text-decoration: none !important;\n      border: 0 !important;\n    }\n</style>\n\n<body style=\"background-color: transparent;border-radius: 18px;\" leftmargin=\"0\" marginwidth=\"0\" topmargin=\"0\"\n    marginheight=\"0\" offset=\"0\">\n    <div style=\"padding: 20px;\">\n        <div style=\"border: 2px solid #e5adb1;border-radius: 18px;\">\n            <div id=\"title\"\n                style=\"height: 100px;width: 100%;background-color:#e5adb1 ;display: flex;justify-content: center;align-items: center;font-weight: bold;font-size: 36px;color: white;border-top-right-radius: 16px;border-top-left-radius: 16px;text-align: center;\">\n                DR LEVENT TUNCER KLİNİK\n            </div>\n\n            <div\n                style=\"background-color: white;padding: 20px;border-bottom-right-radius: 16px;border-bottom-left-radius: 16px;\">\n                <div style=\"text-align: center;font-weight: bold;\">\n                    <p style=\"font-size: 28px;\">Randevu İsteğiniz Oluşturulmuştur</p>\n                </div>\n                <div id=\"button-wrapper\" align=\"center\" valign=\"middle\" class=\"buttonContent\" style=\"padding-top:15px;padding-bottom:15px;padding-right:15px;padding-left:15px;\" >\n           \n                        <a style=\"background-color: #e5adb1;width: 200px;height:46px;border-radius: 12px;color:#FFFFFF;text-decoration:none;font-family:Helvetica,Arial,sans-serif;font-size:20px;line-height:135%;padding: 20px;\"  href=\"https://drleventtuncerklinik.com\" target=\"_blank\">Randevu Detayı</a>\n                </div>\n\n            </div>\n        </div>\n    </div>\n</body>\n\n</html>";

        var mail = new MailData()
        {
            SenderMail = "drleventtuncerklinik@hotmail.com",
            RecieverMail = user.Mail,
            EmailSubject = "Randevu Detayı",
            EmailBody = mailBody
        };

        SendMail(mail);
        return new SuccessResult();
    }

    public async Task<IResults> CreateCompany(CreateCompanyDTO model)
    {
        ;
        var eventModel = _mapper.Map<Company>(model);
        await _companyRepository.AddAsync(eventModel);
        return new SuccessResult();
    }

    public async Task<IResults> DeleteEvent(int eventId)
    {
        var eventItem = await _eventRepository.FindAsync(x => x.Id == eventId);
        if (eventItem == null)
        {
            return new ErrorResult(message: "Randevu bulunamadı");
        }

        eventItem.IsDeleted = true;

        _eventRepository.Update(eventItem);

        return new SuccessResult();
    }
    public async Task<IResults> UpdateEvent(UpdateEventDTO updateEvent)
    {
        var eventItem =await _eventRepository.FindAsync(_ => _.Id == updateEvent.Id);
        var user = await _userRepository.FindAsync(x => x.Id == eventItem.ClientUserId);
        bool isChanged = false;
        if (eventItem==null)
        {
            return new ErrorResult(message: "Randevu bulunamadı");
        }
        if (eventItem.StatusId != updateEvent.statusId)
        {
            eventItem.StatusId = updateEvent.statusId;
            isChanged = true;
            var mail = new MailData();

            if (updateEvent.statusId == 0)
            {
                string mailBody = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n\n<head>\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge,chrome=1\">\n    <title>DR Levent Tuncer</title>\n\n</head>\n<style>\n    #button {\n        display: flex;\n        align-items: center;\n        justify-content: center;\n        text-decoration: none;\n    }\n    #button-wrapper {\n        display: flex;\n        align-items: center;\n        justify-content: center;\n    }\n    #title{\n        text-align: center;\n    }\n    .buttonContent {\n      color: #FFFFFF;\n      font-family: Helvetica;\n      font-size: 18px;\n      font-weight: bold;\n      line-height: 100%;\n      padding: 15px;\n      text-align: center;\n    }\n\n    .buttonContent a {\n      color: #FFFFFF;\n      display: block;\n      text-decoration: none !important;\n      border: 0 !important;\n    }\n</style>\n\n<body style=\"background-color: transparent;border-radius: 18px;\" leftmargin=\"0\" marginwidth=\"0\" topmargin=\"0\"\n    marginheight=\"0\" offset=\"0\">\n    <div style=\"padding: 20px;\">\n        <div style=\"border: 2px solid #e5adb1;border-radius: 18px;\">\n            <div id=\"title\"\n                style=\"height: 100px;width: 100%;background-color:#e5adb1 ;display: flex;justify-content: center;align-items: center;font-weight: bold;font-size: 36px;color: white;border-top-right-radius: 16px;border-top-left-radius: 16px;text-align: center;\">\n                DR LEVENT TUNCER KLİNİK\n            </div>\n\n            <div\n                style=\"background-color: white;padding: 20px;border-bottom-right-radius: 16px;border-bottom-left-radius: 16px;\">\n                <div style=\"text-align: center;font-weight: bold;\">\n                    <p style=\"font-size: 28px;\">Randevunuz Beklemede</p>\n                </div>\n                <div id=\"button-wrapper\" align=\"center\" valign=\"middle\" class=\"buttonContent\" style=\"padding-top:15px;padding-bottom:15px;padding-right:15px;padding-left:15px;\" >\n           \n                        <a style=\"background-color: #e5adb1;width: 200px;height:46px;border-radius: 12px;color:#FFFFFF;text-decoration:none;font-family:Helvetica,Arial,sans-serif;font-size:20px;line-height:135%;padding: 20px;\"  href=\"https://drleventtuncerklinik.com\" target=\"_blank\">Randevu Beklemeye Alındı </a>\n                </div>\n\n            </div>\n        </div>\n    </div>\n</body>\n\n</html>";

                mail = new MailData()
                {
                    SenderMail = "drleventtuncerklinik@hotmail.com",
                    RecieverMail = user.Mail,
                    EmailSubject = "Randevu Beklemede",
                    EmailBody = mailBody
                };
            }
            else if (updateEvent.statusId == 1)
            {

                string mailBody = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n\n<head>\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge,chrome=1\">\n    <title>DR Levent Tuncer</title>\n\n</head>\n<style>\n    #button {\n        display: flex;\n        align-items: center;\n        justify-content: center;\n        text-decoration: none;\n    }\n    #button-wrapper {\n        display: flex;\n        align-items: center;\n        justify-content: center;\n    }\n    #title{\n        text-align: center;\n    }\n    .buttonContent {\n      color: #FFFFFF;\n      font-family: Helvetica;\n      font-size: 18px;\n      font-weight: bold;\n      line-height: 100%;\n      padding: 15px;\n      text-align: center;\n    }\n\n    .buttonContent a {\n      color: #FFFFFF;\n      display: block;\n      text-decoration: none !important;\n      border: 0 !important;\n    }\n</style>\n\n<body style=\"background-color: transparent;border-radius: 18px;\" leftmargin=\"0\" marginwidth=\"0\" topmargin=\"0\"\n    marginheight=\"0\" offset=\"0\">\n    <div style=\"padding: 20px;\">\n        <div style=\"border: 2px solid #e5adb1;border-radius: 18px;\">\n            <div id=\"title\"\n                style=\"height: 100px;width: 100%;background-color:#e5adb1 ;display: flex;justify-content: center;align-items: center;font-weight: bold;font-size: 36px;color: white;border-top-right-radius: 16px;border-top-left-radius: 16px;text-align: center;\">\n                DR LEVENT TUNCER KLİNİK\n            </div>\n\n            <div\n                style=\"background-color: white;padding: 20px;border-bottom-right-radius: 16px;border-bottom-left-radius: 16px;\">\n                <div style=\"text-align: center;font-weight: bold;\">\n                    <p style=\"font-size: 28px;\">Randevunuz Reddedildi</p>\n                </div>\n                <div id=\"button-wrapper\" align=\"center\" valign=\"middle\" class=\"buttonContent\" style=\"padding-top:15px;padding-bottom:15px;padding-right:15px;padding-left:15px;\" >\n           \n                        <a style=\"background-color: #e5adb1;width: 200px;height:46px;border-radius: 12px;color:#FFFFFF;text-decoration:none;font-family:Helvetica,Arial,sans-serif;font-size:20px;line-height:135%;padding: 20px;\"  href=\"https://drleventtuncerklinik.com\" target=\"_blank\">Randevu Beklemeye Alındı </a>\n                </div>\n\n            </div>\n        </div>\n    </div>\n</body>\n\n</html>";

                mail = new MailData()
                {
                    SenderMail = "drleventtuncerklinik@hotmail.com",
                    RecieverMail = user.Mail,
                    EmailSubject = "Randevu Detayı",
                    EmailBody = mailBody
                };
            }
            else
            {

                string mailBody = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n\n<head>\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge,chrome=1\">\n    <title>DR Levent Tuncer</title>\n\n</head>\n<style>\n    #button {\n        display: flex;\n        align-items: center;\n        justify-content: center;\n        text-decoration: none;\n    }\n    #button-wrapper {\n        display: flex;\n        align-items: center;\n        justify-content: center;\n    }\n    #title{\n        text-align: center;\n    }\n    .buttonContent {\n      color: #FFFFFF;\n      font-family: Helvetica;\n      font-size: 18px;\n      font-weight: bold;\n      line-height: 100%;\n      padding: 15px;\n      text-align: center;\n    }\n\n    .buttonContent a {\n      color: #FFFFFF;\n      display: block;\n      text-decoration: none !important;\n      border: 0 !important;\n    }\n</style>\n\n<body style=\"background-color: transparent;border-radius: 18px;\" leftmargin=\"0\" marginwidth=\"0\" topmargin=\"0\"\n    marginheight=\"0\" offset=\"0\">\n    <div style=\"padding: 20px;\">\n        <div style=\"border: 2px solid #e5adb1;border-radius: 18px;\">\n            <div id=\"title\"\n                style=\"height: 100px;width: 100%;background-color:#e5adb1 ;display: flex;justify-content: center;align-items: center;font-weight: bold;font-size: 36px;color: white;border-top-right-radius: 16px;border-top-left-radius: 16px;text-align: center;\">\n                DR LEVENT TUNCER KLİNİK\n            </div>\n\n            <div\n                style=\"background-color: white;padding: 20px;border-bottom-right-radius: 16px;border-bottom-left-radius: 16px;\">\n                <div style=\"text-align: center;font-weight: bold;\">\n                    <p style=\"font-size: 28px;\">Randevunuz Onaylandı</p>\n                </div>\n                <div id=\"button-wrapper\" align=\"center\" valign=\"middle\" class=\"buttonContent\" style=\"padding-top:15px;padding-bottom:15px;padding-right:15px;padding-left:15px;\" >\n           \n                        <a style=\"background-color: #e5adb1;width: 200px;height:46px;border-radius: 12px;color:#FFFFFF;text-decoration:none;font-family:Helvetica,Arial,sans-serif;font-size:20px;line-height:135%;padding: 20px;\"  href=\"https://drleventtuncerklinik.com\" target=\"_blank\">Randevu Beklemeye Alındı </a>\n                </div>\n\n            </div>\n        </div>\n    </div>\n</body>\n\n</html>";

                mail = new MailData()
                {
                    SenderMail = "drleventtuncerklinik@hotmail.com",
                    RecieverMail = user.Mail,
                    EmailSubject = "Randevu Detayı",
                    EmailBody = mailBody
                };
            }

            SendMail(mail);
        }
        if (!eventItem.StartDateTime.Equals(updateEvent.StartDateTime)|| eventItem.EndDateTime.Equals(updateEvent.EndDateTime))
        {
            eventItem.StartDateTime = updateEvent.StartDateTime;
            eventItem.EndDateTime = updateEvent.EndDateTime;
            isChanged = true;
        }
        if (isChanged)
        {
            _eventRepository.Update(eventItem);
        }

        return new SuccessResult();

    }
    public async Task<IResults> ChangeStatus(int statusId, int eventId)
    {
        var eventItem = await _eventRepository.FindAsync(x => x.Id == eventId);
        //buradaki mail gönderme generic olması lazım 
        var user = await _userRepository.FindAsync(x => x.Id == eventItem.ClientUserId);
        if (eventItem == null)
        {
            return new ErrorResult(message: "Randevu bulunamadı");
        }

        eventItem.StatusId = statusId;

        _eventRepository.Update(eventItem);
        var mail = new MailData();

        if (statusId == 0)
        { 
            string mailBody = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n\n<head>\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge,chrome=1\">\n    <title>DR Levent Tuncer</title>\n\n</head>\n<style>\n    #button {\n        display: flex;\n        align-items: center;\n        justify-content: center;\n        text-decoration: none;\n    }\n    #button-wrapper {\n        display: flex;\n        align-items: center;\n        justify-content: center;\n    }\n    #title{\n        text-align: center;\n    }\n    .buttonContent {\n      color: #FFFFFF;\n      font-family: Helvetica;\n      font-size: 18px;\n      font-weight: bold;\n      line-height: 100%;\n      padding: 15px;\n      text-align: center;\n    }\n\n    .buttonContent a {\n      color: #FFFFFF;\n      display: block;\n      text-decoration: none !important;\n      border: 0 !important;\n    }\n</style>\n\n<body style=\"background-color: transparent;border-radius: 18px;\" leftmargin=\"0\" marginwidth=\"0\" topmargin=\"0\"\n    marginheight=\"0\" offset=\"0\">\n    <div style=\"padding: 20px;\">\n        <div style=\"border: 2px solid #e5adb1;border-radius: 18px;\">\n            <div id=\"title\"\n                style=\"height: 100px;width: 100%;background-color:#e5adb1 ;display: flex;justify-content: center;align-items: center;font-weight: bold;font-size: 36px;color: white;border-top-right-radius: 16px;border-top-left-radius: 16px;text-align: center;\">\n                DR LEVENT TUNCER KLİNİK\n            </div>\n\n            <div\n                style=\"background-color: white;padding: 20px;border-bottom-right-radius: 16px;border-bottom-left-radius: 16px;\">\n                <div style=\"text-align: center;font-weight: bold;\">\n                    <p style=\"font-size: 28px;\">Randevunuz Beklemede</p>\n                </div>\n                <div id=\"button-wrapper\" align=\"center\" valign=\"middle\" class=\"buttonContent\" style=\"padding-top:15px;padding-bottom:15px;padding-right:15px;padding-left:15px;\" >\n           \n                        <a style=\"background-color: #e5adb1;width: 200px;height:46px;border-radius: 12px;color:#FFFFFF;text-decoration:none;font-family:Helvetica,Arial,sans-serif;font-size:20px;line-height:135%;padding: 20px;\"  href=\"https://drleventtuncerklinik.com\" target=\"_blank\">Randevu Beklemeye Alındı </a>\n                </div>\n\n            </div>\n        </div>\n    </div>\n</body>\n\n</html>";

            mail = new MailData()
            {
                SenderMail = "drleventtuncerklinik@hotmail.com",
                RecieverMail = user.Mail,
                EmailSubject = "Randevu Beklemede",
                EmailBody = mailBody
            };
        }
        else if (statusId == 1)
        {
            
                        string mailBody = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n\n<head>\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge,chrome=1\">\n    <title>DR Levent Tuncer</title>\n\n</head>\n<style>\n    #button {\n        display: flex;\n        align-items: center;\n        justify-content: center;\n        text-decoration: none;\n    }\n    #button-wrapper {\n        display: flex;\n        align-items: center;\n        justify-content: center;\n    }\n    #title{\n        text-align: center;\n    }\n    .buttonContent {\n      color: #FFFFFF;\n      font-family: Helvetica;\n      font-size: 18px;\n      font-weight: bold;\n      line-height: 100%;\n      padding: 15px;\n      text-align: center;\n    }\n\n    .buttonContent a {\n      color: #FFFFFF;\n      display: block;\n      text-decoration: none !important;\n      border: 0 !important;\n    }\n</style>\n\n<body style=\"background-color: transparent;border-radius: 18px;\" leftmargin=\"0\" marginwidth=\"0\" topmargin=\"0\"\n    marginheight=\"0\" offset=\"0\">\n    <div style=\"padding: 20px;\">\n        <div style=\"border: 2px solid #e5adb1;border-radius: 18px;\">\n            <div id=\"title\"\n                style=\"height: 100px;width: 100%;background-color:#e5adb1 ;display: flex;justify-content: center;align-items: center;font-weight: bold;font-size: 36px;color: white;border-top-right-radius: 16px;border-top-left-radius: 16px;text-align: center;\">\n                DR LEVENT TUNCER KLİNİK\n            </div>\n\n            <div\n                style=\"background-color: white;padding: 20px;border-bottom-right-radius: 16px;border-bottom-left-radius: 16px;\">\n                <div style=\"text-align: center;font-weight: bold;\">\n                    <p style=\"font-size: 28px;\">Randevunuz Reddedildi</p>\n                </div>\n                <div id=\"button-wrapper\" align=\"center\" valign=\"middle\" class=\"buttonContent\" style=\"padding-top:15px;padding-bottom:15px;padding-right:15px;padding-left:15px;\" >\n           \n                        <a style=\"background-color: #e5adb1;width: 200px;height:46px;border-radius: 12px;color:#FFFFFF;text-decoration:none;font-family:Helvetica,Arial,sans-serif;font-size:20px;line-height:135%;padding: 20px;\"  href=\"https://drleventtuncerklinik.com\" target=\"_blank\">Randevu Beklemeye Alındı </a>\n                </div>\n\n            </div>\n        </div>\n    </div>\n</body>\n\n</html>";

            mail = new MailData()
            {
                SenderMail = "drleventtuncerklinik@hotmail.com",
                RecieverMail = user.Mail,
                EmailSubject = "Randevu Detayı",
                EmailBody = mailBody
            };
        }
        else
        {
            string mailBody = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n\n<head>\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge,chrome=1\">\n    <title>DR Levent Tuncer</title>\n\n</head>\n<style>\n    #button {\n        display: flex;\n        align-items: center;\n        justify-content: center;\n        text-decoration: none;\n    }\n    #button-wrapper {\n        display: flex;\n        align-items: center;\n        justify-content: center;\n    }\n    #title{\n        text-align: center;\n    }\n    .buttonContent {\n      color: #FFFFFF;\n      font-family: Helvetica;\n      font-size: 18px;\n      font-weight: bold;\n      line-height: 100%;\n      padding: 15px;\n      text-align: center;\n    }\n\n    .buttonContent a {\n      color: #FFFFFF;\n      display: block;\n      text-decoration: none !important;\n      border: 0 !important;\n    }\n</style>\n\n<body style=\"background-color: transparent;border-radius: 18px;\" leftmargin=\"0\" marginwidth=\"0\" topmargin=\"0\"\n    marginheight=\"0\" offset=\"0\">\n    <div style=\"padding: 20px;\">\n        <div style=\"border: 2px solid #e5adb1;border-radius: 18px;\">\n            <div id=\"title\"\n                style=\"height: 100px;width: 100%;background-color:#e5adb1 ;display: flex;justify-content: center;align-items: center;font-weight: bold;font-size: 36px;color: white;border-top-right-radius: 16px;border-top-left-radius: 16px;text-align: center;\">\n                DR LEVENT TUNCER KLİNİK\n            </div>\n\n            <div\n                style=\"background-color: white;padding: 20px;border-bottom-right-radius: 16px;border-bottom-left-radius: 16px;\">\n                <div style=\"text-align: center;font-weight: bold;\">\n                    <p style=\"font-size: 28px;\">Randevunuz Onaylandı</p>\n                </div>\n                <div id=\"button-wrapper\" align=\"center\" valign=\"middle\" class=\"buttonContent\" style=\"padding-top:15px;padding-bottom:15px;padding-right:15px;padding-left:15px;\" >\n           \n                        <a style=\"background-color: #e5adb1;width: 200px;height:46px;border-radius: 12px;color:#FFFFFF;text-decoration:none;font-family:Helvetica,Arial,sans-serif;font-size:20px;line-height:135%;padding: 20px;\"  href=\"https://drleventtuncerklinik.com\" target=\"_blank\">Randevu Beklemeye Alındı </a>\n                </div>\n\n            </div>\n        </div>\n    </div>\n</body>\n\n</html>";

            mail = new MailData()
            {
                SenderMail = "drleventtuncerklinik@hotmail.com",
                RecieverMail = user.Mail,
                EmailSubject = "Randevu Detayı",
                EmailBody = mailBody
            };
        }

        SendMail(mail);
        return new SuccessResult();
    }

    public Task SendMail(MailData mail)
    {
        var client = new SmtpClient("smtp-mail.outlook.com", 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential("drleventtuncerklinik@hotmail.com", "Bthntncr81.")
        };
        MailMessage message = new MailMessage(mail.SenderMail, mail.RecieverMail, mail.EmailSubject, mail.EmailBody);

        message.IsBodyHtml = true;
        return client.SendMailAsync(message);
    }

    public async Task<IDataResults<ICollection<EventToMonthDTO>>> GetListDayByClientId(DateTime date)
    {
        var userId = GetLoggedUserId();
        //BURASI YIL DA KONTROL ETMESİ LAZIM
        var query = _eventRepository.Where(x =>
            x.IsDeleted && x.ClientUserId == userId || x.AdminUserId == userId && x.StartDateTime.Month == date.Month);


        var data = _mapper.Map<ICollection<EventToMonthDTO>>(await query.ToListAsync());
        return new SuccessDataResult<ICollection<EventToMonthDTO>>(data, data.Count);
    }


    public async Task<IDataResults<ICollection<EventListClientResponseDto>>> ListEventsByUserId(DateTime date)
    {
        var userId = GetLoggedUserId();
        // && ( x.Date>=date.AddHours(3)&&x.Date<=date.AddDays(1).AddHours(3))
        var eventRepo = _eventRepository.Where(x =>
            !x.IsDeleted && (x.ClientUserId == userId || x.AdminUserId == userId) &&
            (x.StartDateTime <= date.AddMonths(1) || x.StartDateTime >= date.AddMonths(-1)));
        var eventTypeRepo = _eventTypeRepository.Where(x => !x.IsDeleted);
        var adminRepo = _userRepository.Where(x => !x.IsDeleted);
        var clientRepo = _userRepository.Where(x => !x.IsDeleted);

        var query = from mal in eventRepo
            join eventType in eventTypeRepo on mal.EventTypeId equals eventType.Id into eventTypeUserLeft
            from eventType in eventTypeUserLeft.DefaultIfEmpty()
            join admin in adminRepo on mal.AdminUserId equals admin.Id into adminTypeUserLeft
            from admin in adminTypeUserLeft.DefaultIfEmpty()
            join client in clientRepo on mal.ClientUserId equals client.Id into clientTypeUserLeft
            from client in clientTypeUserLeft.DefaultIfEmpty()
            select new EventListClientResponseDto
            {
                StartDateTime = mal.StartDateTime,
                EndDateTime = mal.EndDateTime,
                Description = mal.Description,
                AdminUserId = mal.AdminUserId,
                ClientUserId = mal.ClientUserId,
                EventTypeDto = new EventTypeDTO()
                {
                    Duration = eventType.Duration,
                    EventName = eventType.Name,
                    Description = eventType.Description,
                    EventTypeId = eventType.Id,
                    Price = eventType.Price
                },
                Admin = new UserDTO()
                {
                    Name = admin.Name,
                    Surname = admin.Surname,
                    Phone = admin.Phone,
                    Id = admin.Id,
                    Address = admin.Address,
                    Mail = admin.Mail
                },
                Client = new UserDTO()
                {
                    Name = client.Name,
                    Surname = client.Surname,
                    Phone = client.Phone,
                    Id = client.Id,
                    Address = client.Address,
                    Mail = client.Mail
                },
                StatusId = mal.StatusId,
                Id = mal.Id
            };

        var data = _mapper.Map<ICollection<EventListClientResponseDto>>(await query.ToListAsync());
        return new SuccessDataResult<ICollection<EventListClientResponseDto>>(data, data.Count);
    }


    public async Task<IDataResults<ICollection<EventListClientResponseDto>>> ListAllEventsByUserId()
    {
        var userId = GetLoggedUserId();
        // && ( x.Date>=date.AddHours(3)&&x.Date<=date.AddDays(1).AddHours(3))
        var eventRepo = _eventRepository.Where(x =>
            !x.IsDeleted && x.ClientUserId == userId);
        var eventTypeRepo = _eventTypeRepository.Where(x => !x.IsDeleted);
        var adminRepo = _userRepository.Where(x => !x.IsDeleted);
        var clientRepo = _userRepository.Where(x => !x.IsDeleted);

        var query = from mal in eventRepo
            join eventType in eventTypeRepo on mal.EventTypeId equals eventType.Id into eventTypeUserLeft
            from eventType in eventTypeUserLeft.DefaultIfEmpty()
            join admin in adminRepo on mal.AdminUserId equals admin.Id into adminTypeUserLeft
            from admin in adminTypeUserLeft.DefaultIfEmpty()
            join client in clientRepo on mal.ClientUserId equals client.Id into clientTypeUserLeft
            from client in clientTypeUserLeft.DefaultIfEmpty()
            select new EventListClientResponseDto
            {
                StartDateTime = mal.StartDateTime,
                EndDateTime = mal.EndDateTime,
                Description = mal.Description,
                AdminUserId = mal.AdminUserId,
                ClientUserId = mal.ClientUserId,
                EventTypeDto = new EventTypeDTO()
                {
                    Duration = eventType.Duration,
                    EventName = eventType.Name,
                    Description = eventType.Description,
                    EventTypeId = eventType.Id,
                    Price = eventType.Price
                },
                Admin = new UserDTO()
                {
                    Name = admin.Name,
                    Surname = admin.Surname,
                    Phone = admin.Phone,
                    Id = admin.Id,
                    Address = admin.Address,
                    Mail = admin.Mail
                },
                Client = new UserDTO()
                {
                    Name = client.Name,
                    Surname = client.Surname,
                    Phone = client.Phone,
                    Id = client.Id,
                    Address = client.Address,
                    Mail = client.Mail
                },
                StatusId = mal.StatusId,
                Id = mal.Id
            };

        var data = _mapper.Map<ICollection<EventListClientResponseDto>>(await query.ToListAsync());
        return new SuccessDataResult<ICollection<EventListClientResponseDto>>(data, data.Count);
    }

    public async Task<IResults> ChangeEventTime(ChageEventTimeDto eventTime)
    {
        var myEvent = _eventRepository.Where(x => x.Id == eventTime.Id).FirstOrDefault();
        myEvent.StartDateTime = eventTime.StartDateTime;
        myEvent.EndDateTime = eventTime.EndDateTime;
        _eventRepository.Update(myEvent);
        return new SuccessResult();
    }

    public async Task<IDataResults<ICollection<EventListClientResponseDto>>> ListEventsByUserIdByDay(DateTime date)
    {
        var userId = GetLoggedUserId();
        var eventRepo = _eventRepository.Where(x =>
            !x.IsDeleted && (x.ClientUserId == userId || x.AdminUserId == userId) &&
            x.StartDateTime >= date.AddHours(3) && x.EndDateTime <= date.AddDays(1).AddHours(3));
        var eventTypeRepo = _eventTypeRepository.Where(x => !x.IsDeleted);
        var adminRepo = _userRepository.Where(x => !x.IsDeleted);
        var clientRepo = _userRepository.Where(x => !x.IsDeleted);

        var query = from mal in eventRepo
            join eventType in eventTypeRepo on mal.EventTypeId equals eventType.Id into eventTypeUserLeft
            from eventType in eventTypeUserLeft.DefaultIfEmpty()
            join admin in adminRepo on mal.AdminUserId equals admin.Id into adminTypeUserLeft
            from admin in adminTypeUserLeft.DefaultIfEmpty()
            join client in clientRepo on mal.ClientUserId equals client.Id into clientTypeUserLeft
            from client in clientTypeUserLeft.DefaultIfEmpty()
            select new EventListClientResponseDto
            {
                StartDateTime = mal.StartDateTime,
                EndDateTime = mal.EndDateTime,
                Description = mal.Description,
                AdminUserId = mal.AdminUserId,
                ClientUserId = mal.ClientUserId,
                EventTypeDto = new EventTypeDTO()
                {
                    Duration = eventType.Duration,
                    EventName = eventType.Name,
                    Description = eventType.Description,
                    EventTypeId = eventType.Id,
                    Price = eventType.Price
                },
                Admin = new UserDTO()
                {
                    Name = admin.Name,
                    Surname = admin.Surname,
                    Phone = admin.Phone,
                    Id = admin.Id,
                    Address = admin.Address,
                    Mail = admin.Mail
                },
                Client = new UserDTO()
                {
                    Name = client.Name,
                    Surname = client.Surname,
                    Phone = client.Phone,
                    Id = client.Id,
                    Address = client.Address,
                    Mail = client.Mail
                },
                StatusId = mal.StatusId,
                Id = mal.Id
            };

        var data = _mapper.Map<ICollection<EventListClientResponseDto>>(await query.ToListAsync());
        return new SuccessDataResult<ICollection<EventListClientResponseDto>>(data, data.Count);
    }


    public async Task<IDataResults<EventByEventId>> EventDetailByEventId(int eventId)
    {
        var eventRepo = _eventRepository.Where(x => x.Id == eventId).FirstOrDefault();
        var eventType = _eventTypeRepository.Where(x => !x.IsDeleted && x.Id == eventRepo.EventTypeId).FirstOrDefault();
        var client = _userRepository.Where(x => !x.IsDeleted && eventRepo.ClientUserId == x.Id).FirstOrDefault();


        var item = new EventByEventId
        {
            StartDateTime = eventRepo.StartDateTime,
            EndDateTime = eventRepo.EndDateTime,
            Description = eventRepo.Description,
            AdminUserId = eventRepo.AdminUserId,
            ClientUserId = eventRepo.ClientUserId,
            EventTypeDto = new EventTypeDTO()
            {
                Duration = eventType.Duration,
                EventName = eventType.Name,
                Description = eventType.Description,
                EventTypeId = eventType.Id,
                Price = eventType.Price
            },

            Client = new UserDTO()
            {
                Name = client.Name,
                Surname = client.Surname,
                Phone = client.Phone,
                Id = client.Id,
                Address = client.Address,
                Mail = client.Mail
            },
            StatusId = eventRepo.StatusId,
            Id = eventRepo.Id
        };


        return new SuccessDataResult<EventByEventId>(item);
    }

    private int? GetLoggedUserId()
    {
        var userRoleString = _loggedUser.FindFirstValue("Id");
        if (int.TryParse(userRoleString, out var userId))
        {
            return userId;
        }

        return null;
    }

   
}