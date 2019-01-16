using CSSEntity;
using System.Collections.Generic;

namespace CSSService
{
   public interface IEmployeeService : IService<Employee>
    {
        int UpdateEmployee(Employee emp);
        int UpdateEmployeeByAdmin(Employee emp);
        string generateId(string pos, string rdate);
        string generatePass();
        List<Employee> GetAllDrivers(string OfficeLocation);
        int UpdateDriverCurrentLocation(string OfficeId, string EmployeeId);
    }
}
