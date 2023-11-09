namespace GTBack.Core.DTO.Restourant.Request;

public class AdditionAddOrUpdateDTO
{
    public long? Id { get; set; }
    public String Name { get; set; }
    public long TableId { get; set; }
    public long? ClientId { get; set; }
    public bool IsClosed { get; set; }
}