namespace GTBack.Core.Entities.Shopping;

public class Image:BaseEntity
{
    public  virtual Product Product { get; set; }
    public int ProductId { get; set; }
    public string Data { get; set; }
}