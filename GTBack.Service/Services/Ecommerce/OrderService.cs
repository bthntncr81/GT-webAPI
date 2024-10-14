using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Ecommerce.Response;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.DTO.Shopping.Response;
using GTBack.Core.Entities;
using GTBack.Core.Entities.Ecommerce;
using GTBack.Core.Entities.Shopping;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.Services.Shopping;
using GTBack.Repository.Migrations;
using GTBack.Service.Utilities.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using XAct;

namespace GTBack.Service.Services.ShoppingService;

public class OrderService : IEcommerceOrderService
{
    private readonly IService<EcommerceOrder> _service;
    private readonly IService<EcommerceVariantOrderRelation> _orderProdRel;
    private readonly IService<EcommerceClient> _ecommerceClientService;
    private readonly IService<EcommerceVariant> _prodService;
    private readonly IService<EcommerceCompany> _companyService;
    private readonly IService<EcommerceImage> _imageService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ClaimsPrincipal? _loggedUser;
    private readonly IMapper _mapper;
    private readonly IJwtTokenService<BaseRegisterDTO> _tokenService;
    private readonly IMemoryCache _cache;

    public OrderService(IService<EcommerceCompany> companyService, IService<EcommerceImage> imageService, IService<EcommerceVariant> prodService, IService<EcommerceClient> ecommerceClientService, IService<EcommerceVariantOrderRelation> orderProdRel, IMemoryCache cache, IRefreshTokenService refreshTokenService, IJwtTokenService<BaseRegisterDTO> tokenService,
        IHttpContextAccessor httpContextAccessor, IService<EcommerceOrder> service,
        IMapper mapper)
    {
        _mapper = mapper;
        _cache = cache;
        _orderProdRel = orderProdRel;
        _imageService = imageService;
        _companyService = companyService;
        _prodService = prodService;
        _ecommerceClientService = ecommerceClientService;
        _service = service;
        _loggedUser = httpContextAccessor.HttpContext?.User;
        _refreshTokenService = refreshTokenService;
        _tokenService = tokenService;

    }


    public async Task<IDataResults<string>> CreateOrder(OrderDTO model)
    {
        Guid g = Guid.NewGuid();

        var user = await _ecommerceClientService.Where(x => x.Id == model.EcommerceClientId).FirstOrDefaultAsync();

        // Create the order
        var order = new EcommerceOrder
        {
            OrderGuid = GenerateRandomString(10),
            EcommerceClientId = model.EcommerceClientId.HasValue ? model.EcommerceClientId : 0,
            TotalPrice = model.TotalPrice,
            Note = model.Note,
            Status = model.Status,
            OpenAddress = model.OpenAddress,
            Country = model.Country,
            City = model.City,
            District = model.District,
            IyzicoTransactionId = model.IyzicoTransactionId,
            EcommerceCompanyId = user.EcommerceCompanyId
        };

        var addedOrder = await _service.AddAsync(order);

        // Ensure there are no duplicate VariantIds in the model
        var uniqueVariantIds = model.VariantIds.Distinct();

        foreach (var item in uniqueVariantIds)
        {
            var count = model.VariantIds.Count(x => x == item);
            var orderRel = new EcommerceVariantOrderRelation
            {
                EcommerceOrderId = addedOrder.Id,
                EcommerceVariantId = item,
                Count = count,
            };
            await _orderProdRel.AddAsync(orderRel);

        }





        return new SuccessDataResult<string>(addedOrder.OrderGuid);
    }


    public async Task<IDataResults<ICollection<EcommerceOrderListDTO>>> GetOrdersByUserId(int? id, string? orderGuid)
    {

        var orders = new List<EcommerceOrder>();
        if (!orderGuid.IsNullOrEmpty() && orderGuid != "0" && id.HasValue && id != 0)
        {
            // Get orders related to the client
            orders = await _service
               .Where(x => x.EcommerceClientId == id && !x.IsDeleted && x.OrderGuid == orderGuid)
               .Include(o => o.EcommerceVariantOrderRelation)
                   .ThenInclude(ov => ov.EcommerceVariant)
               .ToListAsync();
        }
        else if (id.HasValue && id != 0)
        {
            // Get orders related to the client
            orders = await _service
                .Where(x => x.EcommerceClientId == id && !x.IsDeleted)
                .Include(o => o.EcommerceVariantOrderRelation)
                    .ThenInclude(ov => ov.EcommerceVariant)
                .ToListAsync();
        }
        else if (!orderGuid.IsNullOrEmpty() && orderGuid != "0")
        {
            // Get orders related to the client
            orders = await _service
               .Where(x => !x.IsDeleted && x.OrderGuid == orderGuid)
               .Include(o => o.EcommerceVariantOrderRelation)
                   .ThenInclude(ov => ov.EcommerceVariant)
               .ToListAsync();
        }


        // Get product-order relations (those not marked as deleted)
        var orderRelations = _orderProdRel
            .Where(x => !x.IsDeleted);

        // Get products (those not marked as deleted)
        var products = await _prodService
            .Where(x => !x.IsDeleted)
            .ToListAsync();

        // Get images (those not marked as deleted)
        var images = await _imageService
            .Where(x => !x.IsDeleted)
            .ToListAsync();

        // Get the client information
        var client = await _ecommerceClientService
            .Where(x => !x.IsDeleted && x.Id == id)
            .FirstOrDefaultAsync();

        // Map orders to DTO
        var orderListDTOs = orders.Select(order => new EcommerceOrderListDTO
        {
            Id = order.Id,
            EcommerceClientId = order.EcommerceClientId.HasValue ? order.EcommerceClientId : 0,
            Name = client?.Name, // Handle nullable client data appropriately
            Surname = client?.Surname, // Handle nullable client data appropriately
            Phone = client?.Phone, // Handle nullable client data appropriately
            Mail = client?.Email,
            OpenAddress = order.OpenAddress,
            OrderGuid = order.OrderGuid,
            TotalPrice = order.TotalPrice,
            City = order.City,
            Country = order.Country,
            District = order.District,
            IyzicoTransactionId = order.IyzicoTransactionId,
            Note = order.Note,
            CreatedDate = order.CreatedDate,
            Status = order.Status,
            Products = !order.OrderGuid.IsNullOrEmpty() && orderGuid != "0" ? order.EcommerceVariantOrderRelation.Select(variantOrder => new EcommerceVariantListWithCountDTO
            {
                EcommerceProductId = variantOrder.EcommerceVariant.EcommerceProductId,
                Id = variantOrder.EcommerceVariant.Id,
                VariantCode = variantOrder.EcommerceVariant.VariantCode,
                CreatedDate = variantOrder.CreatedDate,
                Name = variantOrder.EcommerceVariant.Name,
                Description = variantOrder.EcommerceVariant.Description,
                ThumbImage = variantOrder.EcommerceVariant.ThumbImage,
                VariantName = variantOrder.EcommerceVariant.VariantName,
                VariantIndicator = variantOrder.EcommerceVariant.VariantIndicator,
                Stock = variantOrder.EcommerceVariant.Stock,
                Price = variantOrder.EcommerceVariant.Price,
                Images = variantOrder.EcommerceVariant.EcommerceImages
                    .Where(img => !img.IsDeleted)
                    .Select(img => img.Data)
                    .ToList(),
                // You may also include the count of variants ordered, if necessary
                Count = variantOrder.Count
            }).ToList() : null
        }).ToList();
        return new SuccessDataResult<ICollection<EcommerceOrderListDTO>>(orderListDTOs);
    }
    public async Task<IResults> OrderConfirm(long orderId)
    {

        string mailBody =
            "<!doctype html>\n<html lang=\"en-US\">\n\n<head>\n    <meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\" />\n    <title>Reset Password Email Template</title>\n    <meta name=\"description\" content=\"Reset Password Email Template.\">\n    <style type=\"text/css\">\n        a:hover {\n            text-decoration: underline !important;\n        }\n    </style>\n</head>\n\n<body marginheight=\"0\" topmargin=\"0\" marginwidth=\"0\" style=\"margin: 0px; background-color: #f2f3f8;\" leftmargin=\"0\">\n    <!--100% body table-->\n    <table cellspacing=\"0\" border=\"0\" cellpadding=\"0\" width=\"100%\" bgcolor=\"#f2f3f8\"\n        style=\"@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;\">\n        <tr>\n            <td>\n                <table style=\"background-color: #f2f3f8; max-width:670px;  margin:0 auto;\" width=\"100%\" border=\"0\"\n                    align=\"center\" cellpadding=\"0\" cellspacing=\"0\">\n                    <tr>\n                        <td style=\"height:80px;\">&nbsp;</td>\n                    </tr>\n                    <tr>\n                        <td style=\"text-align:center;\">\n                            <a href=\"https://www.boğabutik.com\" title=\"logo\" target=\"_blank\">\n                                <img width=\"150\" src=\"https://www.xn--boabutik-7fb.com/assets/images/logo-no-back.png\" title=\"logo\"\n                                    alt=\"logo\">\n                            </a>\n                        </td>\n                    </tr>\n                    <tr>\n                        <td style=\"height:20px;\">&nbsp;</td>\n                    </tr>\n                    <tr>\n                        <td>\n                            <table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\"\n                                style=\"max-width:670px;background:#fff; border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);\">\n                                <tr>\n                                    <td style=\"height:40px;\">&nbsp;</td>\n                                </tr>\n                                <tr>\n                                    <td style=\"padding:0 35px;\">\n                                        <h1\n                                            style=\"color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;\">\n                                          Sipariş Geldi Hanım</h1>\n                                        <span\n                                            style=\"display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;\"></span>\n                                        <p style=\"color:#455056; font-size:15px;line-height:24px; margin:0;\">\n                                          Sipariş geldi koşşş id si --> " + orderId + "\n    \n                                        </p>\n                                        <a href=\"https://www.boğabutik.com/account/order-detail/" + orderId + "\"\n                                           style=\"background:#2d2e2d;text-decoration:none !important; font-weight:500; margin-top:35px; color:#fff;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;\">\n                                            Siparişi Gör \n                                        </a>\n                                    </td>\n                                </tr>\n                                <tr>\n                                    <td style=\"height:40px;\">&nbsp;</td>\n                                </tr>\n                            </table>\n                        </td>\n                    <tr>\n                        <td style=\"height:20px;\">&nbsp;</td>\n                    </tr>\n                    <tr>\n                        <td style=\"text-align:center;\">\n                            <p\n                                style=\"font-size:14px; color:rgba(69, 80, 86, 0.7411764705882353); line-height:18px; margin:0 0 0;\">\n                                &copy; <strong>www.boğabutik.com</strong></p>\n                        </td>\n                    </tr>\n                    <tr>\n                        <td style=\"height:80px;\">&nbsp;</td>\n                    </tr>\n                </table>\n            </td>\n        </tr>\n    </table>\n    <!--/100% body table-->\n</body>\n\n</html>";

        var mail = new MailData()
        {
            SenderMail = "bogabutik@hotmail.com",
            RecieverMail = "omerbatuhantuncer.workspace@gmail.com",
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
            EnableSsl = true,
            Credentials = new NetworkCredential("bogabutik@hotmail.com", "Bthntncr81.")
        };
        MailMessage message = new MailMessage(mail.SenderMail, mail.RecieverMail, mail.EmailSubject, mail.EmailBody);

        message.IsBodyHtml = true;

        client.SendMailAsync(message);


        return new SuccessDataResult<MailMessage>(message);
    }

    static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"; // Define the character set
        Random random = new Random(); // Create a new instance of the Random class
        StringBuilder result = new StringBuilder(length);

        // Generate the random string
        for (int i = 0; i < length; i++)
        {
            // Get a random index in the range of the character set and append the character at that index to the result
            result.Append(chars[random.Next(chars.Length)]);
        }

        return result.ToString(); // Convert the StringBuilder to a string and return it
    }


    public async Task<IDataResults<ICollection<EcommerceOrderListDTO>>> GetOrderByCompanyId(long? companyId)
    {

        var orders = new List<EcommerceOrder>();

        // Get orders related to the client
        orders = await _service
           .Where(x => !x.IsDeleted && x.EcommerceCompanyId == companyId)
           .Include(o => o.EcommerceVariantOrderRelation)
               .ThenInclude(ov => ov.EcommerceVariant)
           .ToListAsync();




        // Get product-order relations (those not marked as deleted)
        var orderRelations = _orderProdRel
            .Where(x => !x.IsDeleted);

        // Get products (those not marked as deleted)
        var products = await _prodService
            .Where(x => !x.IsDeleted)
            .ToListAsync();

        // Get images (those not marked as deleted)
        var images = await _imageService
            .Where(x => !x.IsDeleted)
            .ToListAsync();



        // Map orders to DTO
        var orderListDTOs = orders.Select(order => new EcommerceOrderListDTO
        {
            Id = order.Id,
            EcommerceClientId = order.EcommerceClientId.HasValue ? order.EcommerceClientId : 0,
            OpenAddress = order.OpenAddress,
            Name = _ecommerceClientService.Where(x => x.Id == order.EcommerceClientId).Select(x => x.Name).FirstOrDefault(), // Handle nullable client data appropriately
            Surname = _ecommerceClientService.Where(x => x.Id == order.EcommerceClientId).Select(x => x.Surname).FirstOrDefault(), // Handle nullable client data appropriately
            Phone = _ecommerceClientService.Where(x => x.Id == order.EcommerceClientId).Select(x => x.Phone).FirstOrDefault(), // Handle nullable client data appropriately
            Mail = _ecommerceClientService.Where(x => x.Id == order.EcommerceClientId).Select(x => x.Email).FirstOrDefault(), // Handle nullable client data appropriately
            OrderGuid = order.OrderGuid,
            TotalPrice = order.TotalPrice,
            City = order.City,
            Country = order.Country,
            District = order.District,
            IyzicoTransactionId = order.IyzicoTransactionId,
            Note = order.Note,
            CreatedDate = order.CreatedDate,
            Status = order.Status,
            Products = order.EcommerceVariantOrderRelation.Select(variantOrder => new EcommerceVariantListWithCountDTO
            {
                EcommerceProductId = variantOrder.EcommerceVariant.EcommerceProductId,
                Id = variantOrder.EcommerceVariant.Id,
                CreatedDate = variantOrder.CreatedDate,
                Name = variantOrder.EcommerceVariant.Name,
                VariantCode = variantOrder.EcommerceVariant.VariantCode,
                Description = variantOrder.EcommerceVariant.Description,
                ThumbImage = variantOrder.EcommerceVariant.ThumbImage,
                VariantName = variantOrder.EcommerceVariant.VariantName,
                VariantIndicator = variantOrder.EcommerceVariant.VariantIndicator,
                Stock = variantOrder.EcommerceVariant.Stock,
                Price = variantOrder.EcommerceVariant.Price,
                Images = variantOrder.EcommerceVariant.EcommerceImages
                    .Where(img => !img.IsDeleted)
                    .Select(img => img.Data)
                    .ToList(),
                // You may also include the count of variants ordered, if necessary
                Count = variantOrder.Count
            }).ToList()
        }).ToList();
        return new SuccessDataResult<ICollection<EcommerceOrderListDTO>>(orderListDTOs);
    }


}