using GTBack.Core.Enums.Restourant;
using GTBack.Core.Models;

namespace GTBack.Core.DTO.Restourant.Response;

public class EmployeeFilterRepresent
{
        
    public RequestFilter Name { get; set; }
    public RequestFilter Surname  { get; set; } 
    public RequestFilter ShiftStart { get; set; }
    public RequestFilter ShiftEnd { get; set; }
    public RequestFilter SalaryType { get; set; }
    public RequestFilter Salary { get; set; }
    public RequestFilter? DeviceId { get; set; }
    public RequestFilter DepartmentId { get; set; }
    public RequestFilter Address { get; set; }
    public RequestFilter Phone { get; set; }
    public RequestFilter Mail { get; set; }
    public RequestFilter RoleList { get; set; }

    public EmployeeFilterRepresent()
    {
        Name = new RequestFilter("Name","string");
        Surname = new RequestFilter("Surname","string");
        Address = new RequestFilter("Address","string");
        Phone = new RequestFilter("Phone","string");
        Mail = new RequestFilter("Mail","string");
        DeviceId = new RequestFilter("DeviceId","int");
        DepartmentId = new RequestFilter("DepartmentId","dropdown");
        Salary = new RequestFilter("Salary","range");
        SalaryType = new RequestFilter("SalaryType","dropdown");
        ShiftEnd = new RequestFilter("ShiftEnd","date");
        ShiftStart = new RequestFilter("ShiftStart","date");
        RoleList = new RequestFilter("RoleList","multiselect");
    
    }

}