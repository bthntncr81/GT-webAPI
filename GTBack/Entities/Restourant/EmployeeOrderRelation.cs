namespace GTBack.Core.Entities.Restourant;

public class EmployeeOrderRelation:BaseEntity
{
    public  long EmployeeId { get; set; }
    public  long AdditionId { get; set; }
    public  Employee Employee { get; set; }
    public  Addition Addition { get; set; }

}