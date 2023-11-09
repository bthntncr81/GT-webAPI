using GTBack.Core.Enums.Restourant;

namespace GTBack.Core.Entities.Restourant;

public class Employee : BaseEntity
{
    public String Name { get; set; }
    public String UserName { get; set; }
    public String Surname { get; set; }
    public String Address { get; set; }
    public String Mail { get; set; }
    public String Phone { get; set; }
    public string PasswordHash { get; set; }
    
    public string? TempPasswordHash { get; set; }
    public string? ApiKey { get; set; }
    public SalaryType? SalaryType { get; set; }

    public DateTime ShiftStart { get; set; }
    public DateTime ShiftEnd { get; set; }
    public int? Salary { get; set; }
    public long? DeviceId { get; set; }
    public long DepartmentId { get; set; }
    public long? CurrencyId { get; set; }
    public Department Department { get; set; }
    public Currency Currency { get; set; }
    public Device? Device { get; set; }
    public virtual ICollection<Reservation> Reservation { get; set; }
    public virtual ICollection<ShiftControl> ShiftControl { get; set; }
    public virtual ICollection<EmployeeRoleRelation> EmployeeRoleRelation { get; set; }
    public virtual ICollection<OrderProcess> OrderProcess { get; set; }
    public virtual ICollection<Order> Order { get; set; }
}