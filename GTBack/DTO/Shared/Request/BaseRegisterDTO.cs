using GTBack.Core.DTO.Restourant;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Enums;
using GTBack.Core.Enums.Ecommerce;

namespace GTBack.Core.DTO;

public class BaseRegisterDTO
{
    public string Name { get; set; }
    public string UserName { get; set; }
    public long Id { get; set; }
    public string Surname { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Mail { get; set; }
    public EcommerceUserType UserTypeId { get; set; }
    public long CompanyId { get; set; }
    
}