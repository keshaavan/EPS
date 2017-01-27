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
    partial class Lookup : IDisposable
    {
        SqlDatabase db;

        public Lookup()
        {
            db = (SqlDatabase)EnterpriseLibraryContainer.Current.GetInstance<Database>(StringMessages.EPSConnectionString);
        }

        public IEnumerable<Entities.Lookup> GetLookupByCategory(int? clientProjectId, string category)
        {
            var lookups = new List<Entities.Lookup>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetLookupByCategory))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, ((clientProjectId.HasValue) ? clientProjectId.Value : new Nullable<int>())));

                sqlParam = new SqlParameter(DBResources.param_Category, SqlDbType.VarChar, 50);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, category));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var lookup = new Entities.Lookup();
                        AssignLookup(reader, lookup);
                        lookups.Add(lookup);
                    }
                }
            }

            return lookups;
        }

        public IEnumerable<Entities.Lookup> GetLookupByClientProjectIdAndCategory(int clientProjectId, string category)
        {
            var lookups = new List<Entities.Lookup>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetLookupByClientProjectIdAndCategory))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                sqlParam = new SqlParameter(DBResources.param_Category, SqlDbType.VarChar, 50);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, category));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var lookup = new Entities.Lookup();
                        AssignLookup(reader, lookup);
                        lookups.Add(lookup);
                    }
                }
            }

            return lookups;
        }

        public Entities.Lookup GetLookupByIdList()
        {
            var lookup = new Entities.Lookup();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetLookupByIdList))
            {
                //SqlParameter sqlParam;
                //sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                //sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, ((clientProjectId.HasValue) ? clientProjectId.Value : new Nullable<int>())));

                //sqlParam = new SqlParameter(DBResources.param_Id, SqlDbType.Int);
                //sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, id));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        AssignLookup(reader, lookup);
                    }
                }
            }

            return lookup;
        }


        public Entities.Lookup GetLookupById(int? clientProjectId, int id)
        {
            var lookup = new Entities.Lookup();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetLookupById))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, ((clientProjectId.HasValue) ? clientProjectId.Value : new Nullable<int>())));

                sqlParam = new SqlParameter(DBResources.param_Id, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, id));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        AssignLookup(reader, lookup);
                    }
                }
            }

            return lookup;
        }

        public void InsertUpdateLookup(Entities.Lookup lookup)
        {
            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_InsertUpdateLookup))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_Id, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, lookup.Id));

                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, lookup.ClientProjectId));

                sqlParam = new SqlParameter(DBResources.param_Name, SqlDbType.VarChar, 255);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, lookup.Name.Trim()));

                sqlParam = new SqlParameter(DBResources.param_Category, SqlDbType.VarChar, 50);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, lookup.Category.Trim()));

                sqlParam = new SqlParameter(DBResources.param_DisplayOrder, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, lookup.DisplayOrder));

                sqlParam = new SqlParameter(DBResources.param_IsActive, SqlDbType.Bit);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, lookup.IsActive));

                db.ExecuteNonQuery(sqlCommand);
            }
        }

        public IEnumerable<Entities.Lookup> GetLookups(int? clientProjectId)
        {
            var lookups = new List<Entities.Lookup>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetLookups))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, ((clientProjectId.HasValue) ? clientProjectId.Value : new Nullable<int>())));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var lookup = new Entities.Lookup();
                        AssignLookup(reader, lookup);
                        lookups.Add(lookup);
                    }
                }
            }

            return lookups;
        }

        public IEnumerable<Entities.Lookup> GetCommentsCategories(int clientProjectId)
        {
            var lookups = new List<Entities.Lookup>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetCommentsCategories))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var lookup = new Entities.Lookup();
                        AssignLookup(reader, lookup);
                        lookups.Add(lookup);
                    }
                }
            }

            return lookups;
        }

        private Entities.Lookup AssignLookup(IDataReader reader, Entities.Lookup lookup)
        {
            int i;

            i = reader.GetOrdinal(DBResources.col_Id);
            if (!reader.IsDBNull(i))
                lookup.Id = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_ClientProjectId);
            if (!reader.IsDBNull(i))
                lookup.ClientProjectId = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_Name);
            if (!reader.IsDBNull(i))
                lookup.Name = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_Category);
            if (!reader.IsDBNull(i))
                lookup.Category = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_DisplayOrder);
            if (!reader.IsDBNull(i))
                lookup.DisplayOrder = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_IsActive);
            if (!reader.IsDBNull(i))
                lookup.IsActive = reader.GetBoolean(i);

            return lookup;
        }

        public void Dispose()
        {
            db = null;
        }
    }
}
