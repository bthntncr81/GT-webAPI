namespace GTBack.Core.Entities.Restourant;

public class Table:BaseEntity
{
    public String Name { get; set; }
    public int TableNumber { get; set; }
    public long? ActiveClientId { get; set; }
    public long? ActiveAdditionId { get; set; }
    public int Capacity { get; set; }
    public int? RowId { get; set; }
    public int? ColumnId { get; set; }
    public long TableAreaId { get; set; }
    public  virtual  TableArea TableArea { get; set; }
    public  virtual  ICollection<Reservation>? Reservation { get; set; }
    public  virtual  ICollection<Addition>? Addition { get; set; }
    
}