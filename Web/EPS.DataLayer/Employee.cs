using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

using EPS.Entities;
using EPS.Resources;
using EPS.Utilities;

namespace EPS.DataLayer
{
    partial class Employee : IDisposable
    {
        SqlDatabase db;

        public Employee()
        {
            db = (SqlDatabase)EnterpriseLibraryContainer.Current.GetInstance<Database>(StringMessages.EPSConnectionString);
        }

        public Entities.Employee GetEmployeeByUserName(string username)
        {
            var employee = new Entities.Employee();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetEmployeeByUsername))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_UserName, SqlDbType.NVarChar, 256);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, username));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        AssignEmployee(reader, employee);
                    }
                }
            }

            return employee;
        }

        public IEnumerable<Entities.Employee> GetEmployees(int clientProjectId)
        {
            var employees = new List<Entities.Employee>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetEmployees))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var employee = new Entities.Employee();
                        AssignEmployee(reader, employee);
                        employees.Add(employee);
                    }
                }
            }

            return employees;
        }
        //June9
        public IEnumerable<Entities.Employee> GetEmployeesList()
        {
            var employees = new List<Entities.Employee>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetEmployees_List))
            {
                //SqlParameter sqlParam;
                //sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                //sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var employee = new Entities.Employee();
                        AssignEmployee(reader, employee);
                        employees.Add(employee);
                    }
                }
            }

            return employees;
        }


        


        public Entities.Employee GetEmployeeByEmployeeLevelId(int clientProjectId, Int64 employeeLevelId)
        {
            var employee = new Entities.Employee();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetEmployeeByEmployeeLevelId))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                sqlParam = new SqlParameter(DBResources.param_Id, SqlDbType.BigInt);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, employeeLevelId));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        AssignEmployee(reader, employee);
                    }
                }
            }

            return employee;
        }

        public Entities.EmployeeLevel GetEmployeeLevelById(int clientProjectId, Int64 id)
        {
            var employeeLevel = new Entities.EmployeeLevel();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetEmployeeLevelById))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                sqlParam = new SqlParameter(DBResources.param_Id, SqlDbType.BigInt);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, id));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        AssignEmployeeLevel(reader, employeeLevel);
                    }
                }
            }

            return employeeLevel;
        }

        public Entities.EmployeeLevel GetEmployeeLevelByActiveEmployeeId(int clientProjectId, Guid employeeId)
        {
            Entities.EmployeeLevel employeeLevel = null;

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetEmployeeLevelByActiveEmployeeId))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                sqlParam = new SqlParameter(DBResources.param_EmployeeId, SqlDbType.UniqueIdentifier);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, employeeId));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        employeeLevel = new Entities.EmployeeLevel();
                        AssignEmployeeLevel(reader, employeeLevel);
                    }
                }
            }

            return employeeLevel;
        }

        //June9
        public IEnumerable<Entities.EmployeeLevel> GetEmployeeLevelByEmployeeIdList()
        {
            var employeeLevels = new List<Entities.EmployeeLevel>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetEmployeeLevelByEmployeeId_List))
            {
                //SqlParameter sqlParam;

                //sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                //sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                //sqlParam = new SqlParameter(DBResources.param_EmployeeId, SqlDbType.UniqueIdentifier);
                //sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, employeeId));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var employeeLevel = new Entities.EmployeeLevel();
                        AssignEmployeeLevel(reader, employeeLevel);
                        employeeLevels.Add(employeeLevel);
                    }
                }
            }

            return employeeLevels;
        }


        public IEnumerable<Entities.EmployeeLevel> GetEmployeeLevelByEmployeeId(int clientProjectId, Guid employeeId)
        {
            var employeeLevels = new List<Entities.EmployeeLevel>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetEmployeeLevelByEmployeeId))
            {
                SqlParameter sqlParam;

                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                sqlParam = new SqlParameter(DBResources.param_EmployeeId, SqlDbType.UniqueIdentifier);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, employeeId));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var employeeLevel = new Entities.EmployeeLevel();
                        AssignEmployeeLevel(reader, employeeLevel);
                        employeeLevels.Add(employeeLevel);
                    }
                }
            }

            return employeeLevels;
        }

        public void InsertUpdateEmployee(Entities.Employee employee)
        {
            using (DbConnection conn = db.CreateConnection())
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();
                DbTransaction trans = conn.BeginTransaction();

                try
                {
                    SqlParameter sqlParam;

                    using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_InsertUpdateEmployee))
                    {
                        sqlParam = new SqlParameter(DBResources.param_Id, SqlDbType.UniqueIdentifier);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, employee.Id));

                        sqlParam = new SqlParameter(DBResources.param_FirstName, SqlDbType.VarChar, 255);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, employee.FirstName.Trim()));

                        sqlParam = new SqlParameter(DBResources.param_LastName, SqlDbType.VarChar, 255);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, employee.LastName.Trim()));

                        sqlParam = new SqlParameter(DBResources.param_LocationId, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, employee.LocationId));

                        sqlParam = new SqlParameter(DBResources.param_IsActive, SqlDbType.Bit);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, (employee.isActive == true) ? 1 : 0));

                        db.ExecuteNonQuery(sqlCommand, trans);
                    }

                    using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_InsertUpdateEmployeeLevel))
                    {
                        sqlParam = new SqlParameter(DBResources.param_EmployeeId, SqlDbType.UniqueIdentifier);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, employee.EmployeeLevel.EmployeeId));

                        sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, employee.EmployeeLevel.ClientProjectId));

                        sqlParam = new SqlParameter(DBResources.param_LevelNumber, SqlDbType.Int);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, employee.EmployeeLevel.LevelNumber));

                        sqlParam = new SqlParameter(DBResources.param_IsAdmin, SqlDbType.Bit);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, (employee.EmployeeLevel.isAdmin == true) ? 1 : 0));

                        sqlParam = new SqlParameter(DBResources.param_IsReportUser, SqlDbType.Bit);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, (employee.EmployeeLevel.IsReportUser == true) ? 1 : 0));

                        sqlParam = new SqlParameter(DBResources.param_IsActive, SqlDbType.Bit);
                        sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, (employee.EmployeeLevel.isActive == true) ? 1 : 0));

                        db.ExecuteNonQuery(sqlCommand, trans);
                    }

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }

        #region "Assign Employee Entities"
        private Entities.Employee AssignEmployee(IDataReader reader, Entities.Employee employee)
        {
            int i;

            i = reader.GetOrdinal(DBResources.col_Id);
            if (!reader.IsDBNull(i))
                employee.Id = reader.GetGuid(i);

            i = reader.GetOrdinal(DBResources.col_UserName);
            if (!reader.IsDBNull(i))
                employee.UserName = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_FirstName);
            if (!reader.IsDBNull(i))
                employee.FirstName = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_LastName);
            if (!reader.IsDBNull(i))
                employee.LastName = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_LocationId);
            if (!reader.IsDBNull(i))
                employee.LocationId = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_IsActive);
            if (!reader.IsDBNull(i))
                employee.isActive = reader.GetBoolean(i);

            return employee;
        }

        private Entities.EmployeeLevel AssignEmployeeLevel(IDataReader reader, Entities.EmployeeLevel employeeLevel)
        {
            int i;

            i = reader.GetOrdinal(DBResources.col_Id);
            if (!reader.IsDBNull(i))
                employeeLevel.Id = reader.GetInt64(i);

            i = reader.GetOrdinal(DBResources.col_EmployeeId);
            if (!reader.IsDBNull(i))
                employeeLevel.EmployeeId = reader.GetGuid(i);

            i = reader.GetOrdinal(DBResources.col_ClientProjectId);
            if (!reader.IsDBNull(i))
                employeeLevel.ClientProjectId = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_LevelNumber);
            if (!reader.IsDBNull(i))
                employeeLevel.LevelNumber = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_IsAdmin);
            if (!reader.IsDBNull(i))
                employeeLevel.isAdmin = reader.GetBoolean(i);

            i = reader.GetOrdinal(DBResources.col_IsReportUser);
            if (!reader.IsDBNull(i))
                employeeLevel.IsReportUser = reader.GetBoolean(i);

            i = reader.GetOrdinal(DBResources.col_IsActive);
            if (!reader.IsDBNull(i))
                employeeLevel.isActive = reader.GetBoolean(i);

            return employeeLevel;
        }
        #endregion

        public void Dispose()
        {
            db = null;
        }
    }
}
