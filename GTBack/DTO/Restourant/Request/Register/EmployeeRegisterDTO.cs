using GTBack.Core.Enums.Restourant;

namespace GTBack.Core.DTO.Restourant.Request;

public class EmployeeRegisterDTO:BaseRegisterDTO
{

    public DateTime ShiftStart { get; set; }
    public DateTime ShiftEnd { get; set; }
    public SalaryType SalaryType { get; set; }
    public int Salary { get; set; }
    public long? DeviceId { get; set; }
    public long DepartmentId { get; set; }
    
    public long CurrencyId { get; set; }
}

