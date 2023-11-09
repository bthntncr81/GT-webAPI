using GTBack.Core.Entities.SharedEntities;

namespace GTBack.Core.Entities;

public class User  : BaseEntity
{
    
    public String Mail { get; set; }
    public String Name { get; set; }
    public String Surname { get; set; }
    public String Phone { get; set; }
    public String Address { get; set; }
    public string PasswordHash { get; set; }
    public long UserTypeId { get; set; }
    
    public long? CompanyId { get; set; }
    
    public  Company Company { get; set; }
     
    public virtual ICollection<RefreshToken>? RefreshTokens { get; set; }
    public virtual ICollection<Event>? AdminEvent { get; set; }
    public virtual ICollection<Event>? ClientEvent { get; set; }
    public virtual ICollection<SpecialAttributeRelation>? BlackListUserRelationsAdmin { get; set; }
    public virtual ICollection<SpecialAttributeRelation>? BlackListUserRelationsClient { get; set; }
    
    public virtual ICollection<FAQ>? Faq { get; set; }
    
    
    
    
    
    
     

}