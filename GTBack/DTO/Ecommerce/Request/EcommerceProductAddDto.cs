using System.Collections.ObjectModel;

namespace GTBack.Core.DTO.Ecommerce;

public class EcommerceProductAddDto
{
    public long? Id { get; set; }
    public string? Category1 { get; set; }
    public string? Category2 { get; set; }
    public string? Category3 { get; set; }
    public long CompanyId { get; set; }
    public long EmployeeId { get; set; }
    public string? Brand { get; set; }
    public string? VariantsName { get; set; }

    
    public List<EcommerceVariantAddDTO> Variants { get; set; }

}