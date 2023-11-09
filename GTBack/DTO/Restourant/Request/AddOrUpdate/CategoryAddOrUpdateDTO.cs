namespace GTBack.Core.DTO.Restourant.Request;

public class CategoryAddOrUpdateDTO
{
    public long Id { get; set; }
    public String Name { get; set; }
    public long MenuId { get; set; }
    public string Image { get; set; }
}