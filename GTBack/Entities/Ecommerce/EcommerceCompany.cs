namespace GTBack.Core.Entities.Ecommerce;

public class EcommerceCompany : BaseEntity
{
    public string? Logo { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? WebAddress { get; set; }
    public string? EmailPassword { get; set; }
    public string? SmtpServer { get; set; }
    public int? SmtpPort { get; set; }
    public string Phone { get; set; }
    public string? Address { get; set; }
    public string? GeoCodeY { get; set; }
    public string? GeoCodeX { get; set; }
    public int ThemeId { get; set; }
    public int? ProductCardId { get; set; }
    public int? DeailPageId { get; set; }
    public int? HeaderId { get; set; }
    public int? FooterId { get; set; }
    public int? AccountPageId { get; set; }
    public int? OrderDetailPageId { get; set; }
    public int? LoginRegisterPageId { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
    public string? VergiNumber { get; set; }
    public string? IyzicoClientId { get; set; }
    public string? IyzicoSecretId { get; set; }
    public string? PrivacyPolicy { get; set; }
    public string? DeliveredAndReturnPolicy { get; set; }
    public string? DistanceSellingContract { get; set; }
    public virtual ICollection<EcommerceEmployee> EcommerceEmployees { get; set; }
    public virtual ICollection<EcommerceOrder> EcommerceOrder { get; set; }
    public virtual ICollection<EcommerceClient> EcommerceClients { get; set; }
}