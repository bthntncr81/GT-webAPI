
using GTBack.Core.DTO;
using GTBack.Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Iyzipay.Model;
using Iyzipay.Request;
using Address = Iyzipay.Model.Address;
using Currency = Iyzipay.Model.Currency;
using Locale = Iyzipay.Model.Locale;
using Options = Iyzipay.Options;
using Microsoft.AspNetCore.SignalR;
using Server.Hubs;


namespace GTBack.WebAPI.Controllers.Pay
{
    public class PaymentController : CustomPaymentBaseController
    {
        private readonly IHubContext<PayHub> _hubContext;

        public PaymentController(IHubContext<PayHub> hubContext)
        {
            _hubContext = hubContext;
        }
  
        
        [Microsoft.AspNetCore.Mvc.HttpPost("Pay")]
        public async Task<IActionResult> Pay(PaymentRequestDTO req)
        {
            Options options = new Options();
            if (req.isDevelopment)
            {
               options=new Options() {
                    ApiKey = "VAxH2UuoOaLKcQY1Q5C7xnI9U1Hm3V0y",
                    SecretKey = "h9zhWxKLbkkb0f4Y0FKltAbLwgrlfWuV",
                    BaseUrl = "https://api.iyzipay.com"
                };
            }
            else
            {
                options=new Options() {
                    ApiKey = "sandbox-ifkcjkaPdtshoWkt36gjOwpZ9Z5XsUZM",
                    SecretKey = "sandbox-0PfKYCdPshA2ZhqfdGq6JxfB5dXQWeqa",
                    BaseUrl = "https://sandbox-api.iyzipay.com"
                };
            }
        
        CreatePaymentRequest request = new CreatePaymentRequest();
        request.Locale = req.locale;
        request.ConversationId = Guid.NewGuid().ToString();
        request.Price = req.price;
        request.PaidPrice = req.paidPrice;
        request.Currency = Currency.TRY.ToString();
        request.Installment = req.installment;
        request.BasketId = req.basketId;
        request.PaymentChannel = req.paymentChannel;
        request.PaymentGroup = req.paymentGroup;
        request.CallbackUrl = "https://localhost:5213/api/Payments/Payment/PayCallBack";
        
        PaymentCard paymentCard = new PaymentCard();
        paymentCard.CardHolderName = req.paymentCard.cardHolderName;
        paymentCard.CardNumber = req.paymentCard.cardNumber;
        paymentCard.ExpireMonth = req.paymentCard.expireMonth;
        paymentCard.ExpireYear = req.paymentCard.expireYear;
        paymentCard.Cvc = req.paymentCard.cvc;
        paymentCard.RegisterCard = req.paymentCard.registerCard;
        request.PaymentCard = paymentCard;
        
        Buyer buyer = new Buyer();
        buyer.Id = req.buyer.id;
        buyer.Name = req.buyer.name;
        buyer.Surname = req.buyer.surname;
        buyer.GsmNumber = req.buyer.gsmNumber;
        buyer.Email = req.buyer.email;
        buyer.IdentityNumber = req.buyer.identityNumber;
        buyer.LastLoginDate = req.buyer.lastLoginDate;
        buyer.RegistrationDate = req.buyer.registrationDate;
        buyer.RegistrationAddress = req.buyer.registrationAddress;
        buyer.Ip = req.buyer.ip;
        buyer.City = req.buyer.city;
        buyer.Country = req.buyer.country;
        buyer.ZipCode = req.buyer.zipCode;
        request.Buyer = buyer;
        
        Address shippingAddress = new Address();
        shippingAddress.ContactName = req.ShippingAddress.ContactName;
        shippingAddress.City = req.ShippingAddress.City;
        shippingAddress.Country = req.ShippingAddress.Country;
        shippingAddress.Description = req.ShippingAddress.Description;
        shippingAddress.ZipCode = req.ShippingAddress.ZipCode;
        request.ShippingAddress = shippingAddress;
        
        Address billingAddress = new Address();
        billingAddress.ContactName = req.BillingAddress.ContactName;
        billingAddress.City = req.BillingAddress.City;
        billingAddress.Country = req.BillingAddress.Country;
        billingAddress.Description = req.BillingAddress.Description;
        billingAddress.ZipCode = req.BillingAddress.ZipCode;
        request.BillingAddress = billingAddress;
        
        // request.PaymentChannel = PaymentChannel.WEB.ToString();
        // request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
        List<BasketItem> basketItems = new List<BasketItem>();

        foreach (var item in req.basketItems)
        {
            BasketItem basketItem = new BasketItem();
            basketItem.Id = item.id;
            basketItem.Name = item.name;
            basketItem.Category1 = item.category1;
            basketItem.Category2 = item.category2;
            basketItem.ItemType =item.itemType;
            basketItem.Price = item.price;
            basketItem.ItemType=BasketItemType.PHYSICAL.ToString();
            basketItems.Add(basketItem);
        }

        request.BasketItems = basketItems;
        ThreedsInitialize threedsInitialize = ThreedsInitialize.Create(request, options);

        return Ok(new { Content = threedsInitialize.HtmlContent, ConversationId = request.ConversationId });
        
    
    }

        
        [Microsoft.AspNetCore.Mvc.HttpPost("PayCallBack")]
        public async Task<IActionResult> PayCallBack([FromForm] IFormCollection collections)
        {
            
            Options options = new()
            {
                ApiKey = "VAxH2UuoOaLKcQY1Q5C7xnI9U1Hm3V0y",
                SecretKey = "h9zhWxKLbkkb0f4Y0FKltAbLwgrlfWuV",
                BaseUrl = "https://api.iyzipay.com"
            };
            CallbackData data = new(
                Status: collections["status"],
                PaymentId: collections["paymentId"],
                ConversationData: collections["conversationData"],
                ConversationId: collections["conversationId"],
                MDStatus: collections["mdStatus"]);

            if(data.Status != "success")
            {
                return BadRequest("Ödeme başarısız oldu!");
            }
            
            CreateThreedsPaymentRequest request = new CreateThreedsPaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId =collections["conversationId"] ;
            request.PaymentId = collections["paymentId"];
            request.ConversationData =collections["conversationData"];

            ThreedsPayment threedsPayment = ThreedsPayment.Create(request, options);

            await _hubContext.Clients.Client(PayHub.TransactionConnections[data.ConversationId]).SendAsync("Receive", data);

            return Ok();
        }
        
        public sealed record CallbackData(
            string Status,
            string PaymentId,
            string ConversationData,
            string ConversationId,
            string MDStatus);
     
        
      
 

            

    }
}