using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreatePayment([FromBody] PaymentRequest paymentRequest)
        {
            try
            {
                if (paymentRequest == null)
                {
                    return BadRequest(new { message = "Payment request is required." });
                }

                // Ensure conversationId is set if not provided
                if (string.IsNullOrEmpty(paymentRequest.conversationId))
                {
                    paymentRequest.conversationId = GenerateRandomString(12);
                }

                var iyzicoPayment = new IyzicoPayment();
                var paymentResponse = iyzicoPayment.CreatePayment(paymentRequest);

                return Ok(paymentResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private string GenerateRandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }

            return new string(result);
        }
    }

    // Models
    public class PaymentRequest
    {
        public string locale { get; set; }
        public string conversationId { get; set; }
        public decimal price { get; set; }
        public decimal paidPrice { get; set; }
        public string currency { get; set; }
        public string basketId { get; set; }
        public string paymentGroup { get; set; }
        public PaymentCard paymentCard { get; set; }
        public Buyer buyer { get; set; }
        public List<BasketItem> basketItems { get; set; }
    }

    public class PaymentCard
    {
        public string cardHolderName { get; set; }
        public string cardNumber { get; set; }
        public string expireMonth { get; set; }
        public string expireYear { get; set; }
        public string cvc { get; set; }
        public string registerCard { get; set; }
    }

    public class Buyer
    {
        public string id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string gsmNumber { get; set; }
        public string email { get; set; }
        public string identityNumber { get; set; }
        public string lastLoginDate { get; set; }
        public string registrationDate { get; set; }
        public string registrationAddress { get; set; }
        public string ip { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string zipCode { get; set; }
    }

    public class BasketItem
    {
        public string id { get; set; }
        public string name { get; set; }
        public string category1 { get; set; }
        public string category2 { get; set; }
        public string itemType { get; set; }
        public decimal price { get; set; }
    }

    public class IyzicoPayment
    {
        private const string BaseUrl = "https://sandbox-api.iyzipay.com";
        private const string ApiKey = "sandbox-ifkcjkaPdtshoWkt36gjOwpZ9Z5XsUZM";
        private const string SecretKey = "sandbox-0PfKYCdPshA2ZhqfdGq6JxfB5dXQWeqa";

        

        public string CreatePayment(PaymentRequest paymentRequest)
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("/payment/auth", Method.POST);
            request.AddHeader("Content-Type", "application/json");

            // Generate a random string for x-iyzi-rnd
            var rnd = GenerateRandomString(12);
            request.AddHeader("x-iyzi-rnd", rnd);

            // Generate authorization header
            var authorization = GenerateAuthorizationHeader(paymentRequest, rnd);
            request.AddHeader("Authorization", authorization);

            var json = JsonConvert.SerializeObject(paymentRequest);
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            return response.Content;
        }

        private string GenerateAuthorizationHeader(PaymentRequest paymentRequest, string rnd)
        {
            var requestString = GenerateRequestString(paymentRequest);
            var hashSha1 = SHA1.Create();
            var hashInput = ApiKey + rnd + SecretKey + requestString;
            var hashBytes = hashSha1.ComputeHash(Encoding.UTF8.GetBytes(hashInput));
            var hashInBase64 = Convert.ToBase64String(hashBytes);
            var authorization = $"IYZWS {ApiKey}:{hashInBase64}";

            return authorization;
        }

        
     
        private string GenerateRequestString(object obj)
        {
            if (obj == null) return "";

            var type = obj.GetType();
            var props = type.GetProperties();
            var requestString = "[";

            foreach (var prop in props)
            {
                var value = prop.GetValue(obj, null);

                if (value == null) continue;

                if (value is IList<BasketItem> basketItems)
                {
                    requestString += prop.Name + "=";
                    foreach (var item in basketItems)
                    {
                        requestString += GenerateRequestString(item) + ", ";
                    }
                    requestString = requestString.TrimEnd(',', ' ') + ",";
                }
                else if (value.GetType().IsClass && !value.GetType().IsPrimitive && !(value is string))
                {
                    requestString += prop.Name + "=" + GenerateRequestString(value) + ",";
                }
                else
                {
                    requestString += prop.Name + "=" + value + ",";
                }
            }

            return requestString.TrimEnd(',') + "]";
        }

        private string GenerateRandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }

            return new string(result);
        }
    }
}