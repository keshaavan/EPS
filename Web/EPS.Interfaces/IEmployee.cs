using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPS.Interfaces
{
    public interface IEmployee
    {
        Entities.Employee GetEmployeeByEmployeeLevelId(int clientProjectId, Int64 employeeLevelId);
        Entities.Employee GetEmployeeByUserName(int clientProjectId, string username);
        IEnumerable<Entities.Employee> GetEmployees(int clientProjectId);

        Entities.EmployeeLevel GetEmployeeLevelById(int clientProjectId, Int64 Id);
        Entities.EmployeeLevel GetEmployeeLevelByActiveEmployeeId(int clientProjectId, Guid employeeId);
        IEnumerable<Entities.EmployeeLevel> GetEmployeeLevelByEmployeeId(int clientProjectId, Guid employeeId);

        void InsertUpdateEmployee(Entities.Employee employee);
    }
}
