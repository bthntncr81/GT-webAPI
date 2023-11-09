using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Enums.Restourant;
using GTBack.Core.Models;

namespace GTBack.Core.DTO.Restourant.Request;

public class EmployeeListFilter
{
 
    public string? Name { get; set; }
    public string?  Surname { get; set; }
    public DateRange?  ShiftStart { get; set; }
    public DateRange?  ShiftEnd { get; set; }
    public SalaryType? SalaryType { get; set; }
    public int?  Salary { get; set; }
    public long? DeviceId { get; set; }
    public long?  DepartmentId { get; set; }
    public string?  Address { get; set; }
    public string?  Phone { get; set; }
    public string?  Mail { get; set; }
    
    public ICollection<int>?  RoleList { get; set; }
}