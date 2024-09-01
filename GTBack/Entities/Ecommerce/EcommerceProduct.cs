namespace GTBack.Core.Entities.Ecommerce;

public class EcommerceProduct:BaseEntity
{
    public string Category1 { get; set; }
    public string? Category2 { get; set; }
    public string? Category3 { get; set; }
    public string? Brand { get; set; }
    public string? VariantsName { get; set; }
    public long EcommerceCompanyId { get; set; }
    public virtual EcommerceCompany EcommerceCompany { get; set; }
    public long EcommerceEmployeeId { get; set; }
    public virtual EcommerceEmployee EcommerceEmployee { get; set; }
    public virtual ICollection<EcommerceProductOrderRelation> ProductOrderRelations { get; set; }
    public virtual ICollection<EcommerceClientFavoriteRelation> ClientFavoriteRelations { get; set; }
   
    public virtual ICollection<EcommerceVariant> Variants { get; set; }
}