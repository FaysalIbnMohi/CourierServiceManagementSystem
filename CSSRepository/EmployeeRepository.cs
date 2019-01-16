using CSSEntity;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CSSRepository
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private DataContext context;

        public EmployeeRepository() { this.context = DataContext.getInstance(); }

        public int UpdateEmployee(Employee emp)
        {
            Employee empToUpdate = this.context.Employees.SingleOrDefault(e => e.Id == emp.Id);
            empToUpdate.Name = emp.Name;
            empToUpdate.Salary = emp.Salary;
            empToUpdate.Birthday = emp.Birthday;
            empToUpdate.Address = emp.Address;
            empToUpdate.PhoneNumber = emp.PhoneNumber;
            empToUpdate.Email = emp.Email;
            empToUpdate.NationalId = emp.NationalId;
            if (emp.OfficeId != "")
                empToUpdate.OfficeId = emp.OfficeId;
            else
                empToUpdate.OfficeId = null;
            empToUpdate.JoinDate = emp.JoinDate;
            
            return this.context.SaveChanges();
        }

        public int UpdateEmployeeByAdmin(Employee emp)
        {
            Employee empToUpdate = this.context.Employees.SingleOrDefault(e => e.Id == emp.Id);
            empToUpdate.Name = emp.Name;
            empToUpdate.Birthday = emp.Birthday;
            empToUpdate.Address = emp.Address;
            empToUpdate.PhoneNumber = emp.PhoneNumber;
            empToUpdate.NationalId = emp.NationalId;
            return this.context.SaveChanges();
        }

        public int getRowCount()
        {
            return this.context.Employees.Count();
        }
        public List<Employee> GetEmployeesForOffice(string division)//new add
        {
            return this.context.Employees.Where(oh => oh.OfficeId.Contains(division)).ToList();
        }
        public string generateId(string pos, string rdate)
        {
            string year = Convert.ToDateTime(rdate).Year.ToString();
            string month = (Convert.ToDateTime(rdate).Month).ToString();
            int count = getRowCount() + 1;
            string EmployeeId = pos[0].ToString() + "-" + month + year[2] + year[3] + "-" + count + Convert.ToDateTime(rdate).TimeOfDay.Hours;
            return EmployeeId;
        }
        public string generatePass()
        {
            Random random = new Random();
            string pass = random.Next(100000, 999999).ToString();
            return pass;
        }
        public List<Employee> GetAllDrivers(string OfficeLocation)//new add for Vehicle
        {
            return this.context.Employees.Where(oh => oh.Position.Contains("Driver") && oh.CurrentLocation == OfficeLocation && oh.CurrentStatus == "Active").ToList();
        }
        public int UpdateDriverCurrentLocation(string OfficeId, string EmployeeId)
        {
            Employee employee = Get(EmployeeId);
            employee.CurrentLocation = OfficeId;
            context.Entry(employee).Property("CurrentLocation").IsModified = true;
            return context.SaveChanges();
        }
    }
}
