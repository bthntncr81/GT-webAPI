namespace GTBack.Core.Entities.Restourant;

public class EmployeeRoleRelation:BaseEntity
{
    public long EmployeeId { get; set; }
    public long RoleId { get; set; }
    public Employee Employee { get; set; }
    public Role Role { get; set; }
    

}