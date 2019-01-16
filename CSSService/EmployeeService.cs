using CSSEntity;
using CSSRepository;
using System.Collections.Generic;


namespace CSSService
{
    public class EmployeeService : Service<Employee>, IEmployeeService
    {
        EmployeeRepository repo = new EmployeeRepository();
        public int UpdateEmployee(Employee emp)
        {
            return repo.UpdateEmployee(emp);
        }

        public int UpdateEmployeeByAdmin(Employee emp)
        {
            return repo.UpdateEmployeeByAdmin(emp);
        }

        public int getRowCount()
        {
            return repo.getRowCount();
        }
        public List<Employee> GetEmployeesForOffice(string division)//new add
        {
            return repo.GetEmployeesForOffice(division);
        }
        public string generateId(string pos, string rdate)
        {
            return repo.generateId(pos, rdate);
        }
        public string generatePass()
        {
            return repo.generatePass();
        }
        public List<Employee> GetAllDrivers(string OfficeLocation)
        {
            return repo.GetAllDrivers(OfficeLocation);
        }
        public int UpdateDriverCurrentLocation(string OfficeId, string EmployeeId)
        {
            return repo.UpdateDriverCurrentLocation(OfficeId, EmployeeId);
        }
    }
}
