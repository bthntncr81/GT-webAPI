using GTBack.Core.Models;

namespace GTBack.Core.DTO.Restourant.Request;

public class AdditionFilterDTO
{
    public String? Name { get; set; }
    public String?TableAreaName { get; set; }
    public int? TableNumber { get; set; }
    public long? ClientId { get; set; }
    public bool? IsClosed { get; set; }
    public DateRange? ClosedDate { get; set; }
    public DateRange? CreatedDate { get; set; }
}