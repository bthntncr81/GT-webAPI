namespace GTBack.Core.Entities.Restourant;

public class Device:BaseEntity
{
    public String Name { get; set; }
    public String DeviceCode { get; set; }
    public  virtual  ICollection<Employee> Employee { get; set; }

}