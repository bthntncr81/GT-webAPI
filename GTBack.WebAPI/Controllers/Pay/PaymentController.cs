
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

            // "VAxH2UuoOaLKcQY1Q5C7xnI9U1Hm3V0y"
            // "h9zhWxKLbkkb0f4Y0FKltAbLwgrlfWuV"
            try
            {
                // Set API options based on the environment
                Options options = new Options
                {
                    ApiKey = req.isDevelopment ? "sandbox-ifkcjkaPdtshoWkt36gjOwpZ9Z5XsUZM" : req.ApiKey,
                    SecretKey = req.isDevelopment ? "sandbox-0PfKYCdPshA2ZhqfdGq6JxfB5dXQWeqa" : req.SecretKey,
                    BaseUrl = req.isDevelopment ? "https://sandbox-api.iyzipay.com" : "https://api.iyzipay.com"
                };

                var num = "";

                if (req.isDevelopment)
                {
                    num = "0";
                }
                else
                {
                    num = "1";
                }
                // Create a payment request
                CreatePaymentRequest request = new CreatePaymentRequest
                {
                    Locale = req.locale,
                    ConversationId = Guid.NewGuid().ToString(),
                    Price = req.price,
                    PaidPrice = req.paidPrice,
                    Currency = Currency.TRY.ToString(),
                    Installment = req.installment,
                    BasketId = req.basketId,
                    PaymentChannel = req.paymentChannel,
                    PaymentGroup = req.paymentGroup,
                    CallbackUrl = "http://localhost:5213/api/Payments/Payment/PayCallBack/" + options.SecretKey + '/' + options.ApiKey + '/' + num
                };

                // Set payment card details
                request.PaymentCard = new PaymentCard
                {
                    CardHolderName = req.paymentCard.cardHolderName,
                    CardNumber = req.paymentCard.cardNumber,
                    ExpireMonth = req.paymentCard.expireMonth,
                    ExpireYear = req.paymentCard.expireYear,
                    Cvc = req.paymentCard.cvc,
                    RegisterCard = req.paymentCard.registerCard
                };

                // Set buyer details
                request.Buyer = new Buyer
                {
                    Id = req.buyer.id,
                    Name = req.buyer.name,
                    Surname = req.buyer.surname,
                    GsmNumber = req.buyer.gsmNumber,
                    Email = req.buyer.email,
                    IdentityNumber = req.buyer.identityNumber,
                    LastLoginDate = req.buyer.lastLoginDate,
                    RegistrationDate = req.buyer.registrationDate,
                    RegistrationAddress = req.buyer.registrationAddress,
                    Ip = req.buyer.ip,
                    City = req.buyer.city,
                    Country = req.buyer.country,
                    ZipCode = req.buyer.zipCode
                };

                // Set shipping and billing addresses
                request.ShippingAddress = new Address
                {
                    ContactName = req.ShippingAddress.ContactName,
                    City = req.ShippingAddress.City,
                    Country = req.ShippingAddress.Country,
                    Description = req.ShippingAddress.Description,
                    ZipCode = req.ShippingAddress.ZipCode
                };

                request.BillingAddress = new Address
                {
                    ContactName = req.BillingAddress.ContactName,
                    City = req.BillingAddress.City,
                    Country = req.BillingAddress.Country,
                    Description = req.BillingAddress.Description,
                    ZipCode = req.BillingAddress.ZipCode
                };

                // Create basket items
                request.BasketItems = req.basketItems.Select(item => new BasketItem
                {
                    Id = item.id,
                    Name = item.name,
                    Category1 = item.category1,
                    Category2 = item.category2,
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                    Price = item.price
                }).ToList();

                // Initialize 3D secure payment
                ThreedsInitialize threedsInitialize = ThreedsInitialize.Create(request, options);

                if (threedsInitialize.Status == "failure")
                {
                    return BadRequest(new { ErrorMessage = threedsInitialize.ErrorMessage, ErrorCode = threedsInitialize.ErrorCode });
                }

                // Return the HTML content for 3D secure redirection
                return Ok(new { Content = threedsInitialize.HtmlContent, ConversationId = request.ConversationId });
            }
            catch (Exception ex)
            {
                // Catch any exceptions and return a detailed error message
                return StatusCode(500, new { Error = "An error occurred while processing the payment.", Details = ex.Message });
            }
        }


        [Microsoft.AspNetCore.Mvc.HttpPost("PayCallBack/{SecretKey}/{ClientId}/{isDev}")]
        public async Task<IActionResult> PayCallBack([FromForm] IFormCollection collections, string SecretKey, string ClientId, string isDev)
        {

            Options options = new()
            {
                ApiKey = ClientId,
                SecretKey = SecretKey,
                BaseUrl = isDev == "0" ? "https://sandbox-api.iyzipay.com" : "https://api.iyzipay.com"
            };
            CallbackData data = new(
                Status: collections["status"],
                PaymentId: collections["paymentId"],
                ConversationData: collections["conversationData"],
                ConversationId: collections["conversationId"],
                MDStatus: collections["mdStatus"]);

            if (data.Status != "success")
            {


                return BadRequest("Ödeme başarısız oldu!");
            }

            CreateThreedsPaymentRequest request = new CreateThreedsPaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = collections["conversationId"];
            request.PaymentId = collections["paymentId"];
            request.ConversationData = collections["conversationData"];

            ThreedsPayment threedsPayment = ThreedsPayment.Create(request, options);

            if (threedsPayment.Status == "failure")
            {
                await _hubContext.Clients.Client(PayHub.TransactionConnections[data.ConversationId]).SendAsync("Receive", threedsPayment.ErrorMessage);

            }
            else
            {
                if (PayHub.TransactionConnections.ContainsKey(data.ConversationId))
                {
                    await _hubContext.Clients.Client(PayHub.TransactionConnections[data.ConversationId])
                        .SendAsync("Receive", data);
                }
                else
                {
                    // Handle the missing key scenario (e.g., log a message or take another action)
                    Console.WriteLine("dasdasdasd");
                }
            }

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