namespace GTBack.Core.Entities.Restourant;

public class Role:BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public virtual ICollection<EmployeeRoleRelation> EmployeeRoleRelation { get; set; }
    
}