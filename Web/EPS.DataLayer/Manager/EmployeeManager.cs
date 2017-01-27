using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EPS.Interfaces;

namespace EPS.DataLayer
{
    public partial class EmployeeManager : IEmployee, IDisposable
    {
        Employee employeeObject;

        public EmployeeManager()
        {
            employeeObject = new Employee();
        }

        public Entities.Employee GetEmployeeByUserName(int clientProjectId, string username)
        {
            try
            {
                var employee = employeeObject.GetEmployeeByUserName(username);
                var employeeLevel = GetEmployeeLevelByActiveEmployeeId(clientProjectId, employee.Id);

                if (employeeLevel == null)
                    return null;
                else
                {
                    var lookupManager = new LookupManager();
                    employee.Location = lookupManager.GetLookupById(null, employee.LocationId);

                    employee.EmployeeLevel = employeeLevel;

                    return employee;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Entities.EmployeeLevel GetEmployeeLevelByActiveEmployeeId(int clientProjectId, Guid employeeId)
        {
            try
            {
                var empLevelObject = employeeObject.GetEmployeeLevelByActiveEmployeeId(clientProjectId, employeeId);

                if (empLevelObject != null)
                {
                    var clientProjectManager = new ClientProjectManager();
                    empLevelObject.ClientProject = clientProjectManager.GetClientProjectById(clientProjectId);
                }

                return empLevelObject;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Entities.Employee GetEmployeeByEmployeeLevelId(int clientProjectId, Int64 employeeLevelId)
        {
            try
            {
                var employee = employeeObject.GetEmployeeByEmployeeLevelId(clientProjectId, employeeLevelId);

                var lookupManager = new LookupManager();
                employee.Location = lookupManager.GetLookupById(null, employee.LocationId);

                employee.EmployeeLevel = GetEmployeeLevelById(clientProjectId, employeeLevelId);

                return employee;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Entities.EmployeeLevel GetEmployeeLevelById(int clientProjectId, Int64 id)
        {
            try
            {
                return employeeObject.GetEmployeeLevelById(clientProjectId, id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertUpdateEmployee(Entities.Employee employee)
        {
            try
            {
                employeeObject.InsertUpdateEmployee(employee);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Entities.EmployeeLevel> GetEmployeeLevelByEmployeeId(int clientProjectId, Guid employeeId)
        {
            try
            {
                var employeeLevels = employeeObject.GetEmployeeLevelByEmployeeId(clientProjectId, employeeId);

                foreach (var employeeLevel in employeeLevels)
                {
                    var clientProjectManager = new ClientProjectManager();
                    employeeLevel.ClientProject = clientProjectManager.GetClientProjectById(clientProjectId);
                }

                return employeeLevels;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<Entities.EmployeeLevel> GetEmployeeLevelByEmployeeIdList()
        {
            try
            {
                var employeeLevels = employeeObject.GetEmployeeLevelByEmployeeIdList();

                foreach (var employeeLevel in employeeLevels)
                {
                    var clientProjectManager = new ClientProjectManager();
                    employeeLevel.ClientProject = clientProjectManager.GetClientProjectByIdList();
                }

                return employeeLevels;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<Entities.Employee> GetEmployees(int clientProjectId)
        {
            try
            {
                var employees = employeeObject.GetEmployees(clientProjectId);

                foreach (var employee in employees)
                {
                    var lookupManager = new LookupManager();
                    employee.AdminEmployeeLevels = GetEmployeeLevelByEmployeeId(clientProjectId, employee.Id);
                    employee.Location = lookupManager.GetLookupById(null, employee.LocationId);
                }

                return employees;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //June9
        public IEnumerable<Entities.Employee> GetEmployeesList()
        {
            try
            {
                var employees = employeeObject.GetEmployeesList();

                foreach (var employee in employees)
                {
                    var lookupManager = new LookupManager();
                    employee.AdminEmployeeLevels = GetEmployeeLevelByEmployeeIdList();
                   employee.Location = lookupManager.GetLookupByIdList();
                }

                return employees;
            }
            catch (Exception)
            {
                throw;
            }
        }

        

        public void Dispose()
        {
            employeeObject = null;
        }
    }
}
