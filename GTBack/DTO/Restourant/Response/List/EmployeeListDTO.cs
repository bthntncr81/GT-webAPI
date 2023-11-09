using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.Enums.Restourant;

namespace GTBack.Core.DTO.Restourant.Response;

public class EmployeeListDTO
{
    public long? Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime ShiftStart { get; set; }
    public DateTime ShiftEnd { get; set; }
    public SalaryType? SalaryType { get; set; }
    public int? Salary { get; set; }
    public long? DeviceId { get; set; }
    public long DepartmentId { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Mail { get; set; }
    public ICollection<RoleList> RoleList { get; set; }
}