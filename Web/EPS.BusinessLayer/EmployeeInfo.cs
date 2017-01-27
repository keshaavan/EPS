using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EPS.Utilities;
using EPS.Resources;
using EPS.Entities;
using EPS.DataLayer;

using System.Web.Security;

using EPI.Compliance;

namespace EPS.BusinessLayer
{
    public partial class EmployeeInfo : IDisposable
    {
        EmployeeManager manager;

        public EmployeeInfo()
        {
            manager = new EmployeeManager();
        }

        public Entities.Employee GetEmployeeinProject(int clientProjectId, string username)
        {
            try
            {
                return manager.GetEmployeeByUserName(clientProjectId, username);
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
                manager.InsertUpdateEmployee(employee);
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
                return manager.GetEmployees(clientProjectId);
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
                return manager.GetEmployeesList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        


        public Employee GetEmployeeByEmployeeLevelId(int clientProjectId, Int64 employeeLevelId)
        {
            try
            {
                return manager.GetEmployeeByEmployeeLevelId(clientProjectId, employeeLevelId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Boolean UpdatePassword(string username, string oldPassword, string newPassword, int previousPasswordCount)
        {
            try
            {
                return ASPNetMembership.UpdatePassword(username, oldPassword, newPassword, previousPasswordCount);
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Boolean ResetPassword(string username, string newPassword, int previousPasswordCount)
        {
            try
            {
                return ASPNetMembership.ResetPassword(username, newPassword, previousPasswordCount);
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Boolean Logout(string username)
        {
            try
            {
                return ASPNetMembership.LogOut(username);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            manager = null;
        }
    }
}