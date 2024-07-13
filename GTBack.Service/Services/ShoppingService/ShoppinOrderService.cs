using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.DTO.Shopping.Response;
using GTBack.Core.Entities;
using GTBack.Core.Entities.Shopping;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.Services.Shopping;
using GTBack.Service.Utilities.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using XAct;

namespace GTBack.Service.Services.ShoppingService;

public class ShoppinOrderService:IShoppingOrderService
{
    private readonly IService<ShoppingOrder> _service;
    private readonly IService<Address> _addressService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ClaimsPrincipal? _loggedUser;
    private readonly IMapper _mapper;
    private readonly  IJwtTokenService<BaseRegisterDTO> _tokenService;
    private readonly IMemoryCache _cache;

    public ShoppinOrderService( IService<Address> addressService,IMemoryCache cache,IRefreshTokenService refreshTokenService,  IJwtTokenService<BaseRegisterDTO> tokenService,
        IHttpContextAccessor httpContextAccessor, IService<ShoppingOrder> service,
        IMapper mapper)
    {
        _mapper = mapper;
        _cache = cache;
        _service = service;
        _addressService = addressService;
        _loggedUser = httpContextAccessor.HttpContext?.User;
        _refreshTokenService = refreshTokenService;
        _tokenService = tokenService;
        
    }
    
    
    public async Task<IDataResults<OrderConfirmDTO>> CreateOrder(OrderConfirmDTO model)
    {
        Guid g = Guid.NewGuid();
        model.OrderGuid = g.ToString();
        if (!model.AddressId.IsNull())
        {
            var address = _mapper.Map<Address>(model.Address);  
          var addressElement= await  _addressService.AddAsync(address);
          model.AddressId = addressElement.Id;
        }
        var data = _mapper.Map<ShoppingOrder>(model); 
        
       var order= await  _service.AddAsync(data);
        OrderConfirm(order.Id);
        return new SuccessDataResult<OrderConfirmDTO>(model);
    }
    
    
    public async Task<IDataResults<ICollection<ShoppingOrderListDTO>>> GetOrdersByUserId(int id)
    {
     
       var orderRepo=   _service.Where(x=>x.ShoppingUserId==id);
       var addressRepo=   _addressService.Where(x=>!x.IsDeleted);

       var query = from order in orderRepo
           join address in addressRepo on order.AddressId equals address.Id into addressLeft
           from address in addressLeft.DefaultIfEmpty()
           select new ShoppingOrderListDTO()
           {        
               Id=order.Id,
               BasketJsonDetail = order.BasketJsonDetail,
               ShoppingUserId = order.ShoppingUserId.IsNull() ? order.ShoppingUserId:null,
               OrderGuid = order.OrderGuid,
               TotalPrice = order.TotalPrice,
               OrderNote = order.OrderNote,
               Status = order.Status,
               Name = order.Name,
               Surname = order.Surname,
               CreatedDate = order.CreatedDate,
               Phone = order.Phone,
               Mail = order.Mail,
               Address = new AddressResponseDTO
               {
                   Name = address.Name,
                   City = address.City,
                   District = address.District,
                   OpenAddress = address.OpenAddress,
                   Country = address.Country,
               }
               
           };
       


       var orderList = await query.ToListAsync();



        
        return new SuccessDataResult<ICollection<ShoppingOrderListDTO>>(orderList);
    }

    
    public async Task<IDataResults<ICollection<ShoppingOrderListDTO>>> GetOrderByOrderId(int id)
    {
     
        var orderRepo=   _service.Where(x=>x.Id==id);
        var addressRepo=   _addressService.Where(x=>!x.IsDeleted);

        var query = from order in orderRepo
            join address in addressRepo on order.AddressId equals address.Id into addressLeft
            from address in addressLeft.DefaultIfEmpty()
            select new ShoppingOrderListDTO()
            {        
                Id=order.Id,
                BasketJsonDetail = order.BasketJsonDetail,
                ShoppingUserId = order.ShoppingUserId.IsNull() ? order.ShoppingUserId:null,
                OrderGuid = order.OrderGuid,
                TotalPrice = order.TotalPrice,
                OrderNote = order.OrderNote,
                Status = order.Status,
                Name = order.Name,
                Surname = order.Surname,
                CreatedDate = order.CreatedDate,
                Phone = order.Phone,
                Mail = order.Mail,
                Address = new AddressResponseDTO
                {
                    Name = address.Name,
                    City = address.City,
                    District = address.District,
                    OpenAddress = address.OpenAddress,
                    Country = address.Country,
                }
               
            };
       
 
        var orderList = await query.ToListAsync();



        
        return new SuccessDataResult<ICollection<ShoppingOrderListDTO>>(orderList);
    }
    
    
    
     public async Task<IResults> OrderConfirm(long orderId)
    {
        
        string mailBody =
            "<!doctype html>\n<html lang=\"en-US\">\n\n<head>\n    <meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\" />\n    <title>Reset Password Email Template</title>\n    <meta name=\"description\" content=\"Reset Password Email Template.\">\n    <style type=\"text/css\">\n        a:hover {\n            text-decoration: underline !important;\n        }\n    </style>\n</head>\n\n<body marginheight=\"0\" topmargin=\"0\" marginwidth=\"0\" style=\"margin: 0px; background-color: #f2f3f8;\" leftmargin=\"0\">\n    <!--100% body table-->\n    <table cellspacing=\"0\" border=\"0\" cellpadding=\"0\" width=\"100%\" bgcolor=\"#f2f3f8\"\n        style=\"@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;\">\n        <tr>\n            <td>\n                <table style=\"background-color: #f2f3f8; max-width:670px;  margin:0 auto;\" width=\"100%\" border=\"0\"\n                    align=\"center\" cellpadding=\"0\" cellspacing=\"0\">\n                    <tr>\n                        <td style=\"height:80px;\">&nbsp;</td>\n                    </tr>\n                    <tr>\n                        <td style=\"text-align:center;\">\n                            <a href=\"https://www.boğabutik.com\" title=\"logo\" target=\"_blank\">\n                                <img width=\"150\" src=\"https://www.xn--boabutik-7fb.com/assets/images/logo-no-back.png\" title=\"logo\"\n                                    alt=\"logo\">\n                            </a>\n                        </td>\n                    </tr>\n                    <tr>\n                        <td style=\"height:20px;\">&nbsp;</td>\n                    </tr>\n                    <tr>\n                        <td>\n                            <table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\"\n                                style=\"max-width:670px;background:#fff; border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);\">\n                                <tr>\n                                    <td style=\"height:40px;\">&nbsp;</td>\n                                </tr>\n                                <tr>\n                                    <td style=\"padding:0 35px;\">\n                                        <h1\n                                            style=\"color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;\">\n                                          Sipariş Geldi Hanım</h1>\n                                        <span\n                                            style=\"display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;\"></span>\n                                        <p style=\"color:#455056; font-size:15px;line-height:24px; margin:0;\">\n                                          Sipariş geldi koşşş id si --> " + orderId +"\n    \n                                        </p>\n                                        <a href=\"https://www.boğabutik.com/account/order-detail/" + orderId +"\"\n                                           style=\"background:#2d2e2d;text-decoration:none !important; font-weight:500; margin-top:35px; color:#fff;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;\">\n                                            Siparişi Gör \n                                        </a>\n                                    </td>\n                                </tr>\n                                <tr>\n                                    <td style=\"height:40px;\">&nbsp;</td>\n                                </tr>\n                            </table>\n                        </td>\n                    <tr>\n                        <td style=\"height:20px;\">&nbsp;</td>\n                    </tr>\n                    <tr>\n                        <td style=\"text-align:center;\">\n                            <p\n                                style=\"font-size:14px; color:rgba(69, 80, 86, 0.7411764705882353); line-height:18px; margin:0 0 0;\">\n                                &copy; <strong>www.boğabutik.com</strong></p>\n                        </td>\n                    </tr>\n                    <tr>\n                        <td style=\"height:80px;\">&nbsp;</td>\n                    </tr>\n                </table>\n            </td>\n        </tr>\n    </table>\n    <!--/100% body table-->\n</body>\n\n</html>";

        var  mail = new MailData()
            {
                SenderMail = "bogabutik@hotmail.com",
                RecieverMail ="omerbatuhantuncer.workspace@gmail.com",
                EmailSubject = "Birileri Şifresini Unutmuş!",
                EmailBody = mailBody
            };

            SendMail(mail);
            return new SuccessResult();
    }
    public async Task<IResults> SendMail(MailData mail)
    {
        var client = new SmtpClient("smtp-mail.outlook.com", 587)
        {
            EnableSsl =true,
            Credentials = new NetworkCredential("bogabutik@hotmail.com","Bthntncr81.")
        };
        MailMessage message = new MailMessage(mail.SenderMail, mail.RecieverMail, mail.EmailSubject, mail.EmailBody);

        message.IsBodyHtml = true;

        client.SendMailAsync(message);

        
        return new SuccessDataResult<MailMessage>(message);
    }

}