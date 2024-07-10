using Iyzipay.Model;

namespace GTBack.Core.DTO;

public class PaymentRequestDTO
{
    public string locale { get; set; }
    public string conversationId { get; set; }
    public string price { get; set; }
    public int installment { get; set; }
    public bool isDevelopment { get; set; }
    public string paidPrice { get; set; }
    public string currency { get; set; }
    public string basketId { get; set; }
  
    public string paymentGroup { get; set; }
    public string paymentChannel { get; set; }
    public PaymentCardDTO paymentCard { get; set; }
    public BuyerDTO buyer { get; set; }
    public AddressDTO ShippingAddress { get; set; }
    public AddressDTO BillingAddress { get; set; }
    public List<BasketItemDTO> basketItems { get; set; }
}