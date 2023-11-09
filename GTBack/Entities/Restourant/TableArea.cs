namespace GTBack.Core.Entities.Restourant;

public class TableArea:BaseEntity
{
    public String Name { get; set; }
    public int? RowCount { get; set; } 
    public int? ColumnCount { get; set; } 
    public long RestoCompanyId { get; set; }
    public RestoCompany RestoCompany { get; set; }
    public  virtual  ICollection<Table>? Table { get; set; }
    
}