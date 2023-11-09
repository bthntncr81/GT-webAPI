using GTBack.Core.Entities.SharedEntities;

namespace GTBack.Core.Entities;

public class EventType:BaseEntity
{
    
    public String Description { get; set; }
    public String Name { get; set; }
    public int Price { get; set; }
    public int Duration { get; set; }
    public virtual ICollection<EventTypeCompanyRelation>  EventTypeCompanyRelation { get; set; }
    public virtual ICollection<Event>  Event { get; set; }
}