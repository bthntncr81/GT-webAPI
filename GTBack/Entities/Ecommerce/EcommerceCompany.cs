namespace GTBack.Core.Entities.Ecommerce;

public class EcommerceCompany:BaseEntity
{
    public string? Logo { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string? Address { get; set; }
    public string? GeoCodeY { get; set; }
    public string? GeoCodeX { get; set; }
    public int ThemeId { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
    public string? VergiNumber { get; set; }
    public string? IyzicoClientId { get; set; }
    public string? IyzicoSecretId { get; set; }
    public string? PrivacyPolicy { get; set; }
    public string? DeliveredAndReturnPolicy { get; set; }
    public string? DistanceSellingContract { get; set; }
    public virtual ICollection<EcommerceEmployee> EcommerceEmployees { get; set; }
    public virtual ICollection<EcommerceClient> EcommerceClients { get; set; }
}