namespace GTBack.Core.DTO;

public class PaymentCardDTO
{
    public string cardHolderName { get; set; }
    public string cardNumber { get; set; }
    public string expireMonth { get; set; }
    public string expireYear { get; set; }
    public string cvc { get; set; }
    public int? registerCard { get; set; }
}