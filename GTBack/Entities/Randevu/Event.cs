using System.ComponentModel.DataAnnotations.Schema;

namespace GTBack.Core.Entities;

public class Event :BaseEntity
{
    public DateTime StartDateTime { get; set; }
    
    public DateTime EndDateTime { get; set; }
    
    public String Description { get; set; }
    
    public int  StatusId { get; set; }
    public long AdminUserId { get; set; }
    
    public long? CurrencyId { get; set; }
    public long ClientUserId { get; set; }
    
    public long EventTypeId { get; set; }
    public virtual User  AdminUser { get; set; }
    
    public virtual EventType  EventType { get; set; }
    public virtual User ClientUser { get; set; }


  
    

}