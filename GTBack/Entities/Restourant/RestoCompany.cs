namespace GTBack.Core.Entities.Restourant;

public class RestoCompany :BaseEntity
{
    public String Name { get; set; }
    public String Address { get; set; }
    public String Mail { get; set; }
    public String Phone { get; set; }
    public double? Lat { get; set; }
    public double? Lng { get; set; }
    public long? MenuId { get; set; }
    public  Menu Menu { get; set; }
    public  virtual  ICollection<Department> Department { get; set; }
    public  virtual  ICollection<TableArea> TableArea { get; set; }
}