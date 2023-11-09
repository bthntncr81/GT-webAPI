namespace GTBack.Core.Entities.Restourant;

public class Reservation:BaseEntity
{       
    public DateTime ReservationDateTime { get; set; }
    public DateTime ReservationCancelDate { get; set; }
    public int? Deposit { get; set; }
    public long ClientId { get; set; }
    public long? TableId { get; set; }
    public long EmployeeId { get; set; }
    public Client Client { get; set; }
    public Employee Employee { get; set; }
    public Table? Table { get; set; }


    
}