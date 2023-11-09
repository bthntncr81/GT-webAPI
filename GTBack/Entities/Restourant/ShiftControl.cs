namespace GTBack.Core.Entities.Restourant;

public class ShiftControl:BaseEntity
{
    public DateTime EnterDate { get; set; }
    public DateTime LeaveDate { get; set; }
    public long EmployeeId { get; set; }
    public Employee Employee { get; set; }

}