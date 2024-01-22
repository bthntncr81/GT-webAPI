namespace GTBack.Core.Entities.Shopping;

public class Image:BaseEntity
{

    public string Data { get; set; }
    public long ProductId { get; set; }
    public Product Product { get; set; }
}