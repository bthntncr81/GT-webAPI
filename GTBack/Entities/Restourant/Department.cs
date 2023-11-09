namespace GTBack.Core.Entities.Restourant;

public class Department:BaseEntity
{
    public String Name { get; set; }
    public String? Mail { get; set; }
    public String? Phone { get; set; }
    public long RestoCompanyId { get; set; }
    public  RestoCompany RestoCompany { get; set; }
    public  virtual  ICollection<Employee> Employee { get; set; }
    public  virtual  ICollection<TableArea> TableArea { get; set; }
}