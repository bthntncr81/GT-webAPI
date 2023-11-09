namespace GTBack.Core.Entities.Restourant;

public class Addition:BaseEntity
{
    public String Name { get; set; }
    public long TableId { get; set; }
    public long? ClientId { get; set; }
    public DateTime? ClosedDate { get; set; }
    public bool IsClosed { get; set; }
    public Table Table { get; set; }
    public Client? Client { get; set; }
    public  virtual  ICollection<Order>? Order { get; set; }
    public  virtual  ICollection<Payment>? Payment { get; set; }
    
}