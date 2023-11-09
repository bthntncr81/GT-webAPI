using GTBack.Core.Entities.SharedEntities;

namespace GTBack.Core.Entities;

public class SpecialAttributeRelation:BaseEntity
{
    public long AdminUserId { get; set; }
    public long ClientUserId { get; set; }
    
    public virtual User AdminUser { get; set; }

    
    public virtual User ClientUser { get; set; }
    
    public long SpecialAttributeId { get; set; }

}