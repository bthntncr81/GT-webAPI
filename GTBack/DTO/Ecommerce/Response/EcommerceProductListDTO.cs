namespace GTBack.Core.DTO.Ecommerce.Response;

public class EcommerceProductListDTO
{
    public long? Id { get; set; }
    public string Category1 { get; set; }
    public string Category2 { get; set; }
    public string Category3 { get; set; }
    public long CompanyId { get; set; }
    public long EmployeeId { get; set; }
    public string Brand { get; set; }
    public string VariantName { get; set; }
    
    public List<EcommerceVariantListDTO> Variants { get; set; }

}