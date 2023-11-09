using GTBack.Core.Entities.SharedEntities;

namespace GTBack.Core.Entities;

public class EventTypeCompanyRelation:BaseEntity
{
 
    public long EventTypeId { get; set; }
    public long CompanyId { get; set; }
    public virtual EventType EventType{ get; set; }
    public virtual Company Company { get; set; }


}