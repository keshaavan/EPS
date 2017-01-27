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
    partial class Comments: IDisposable
    {
        SqlDatabase db;

        public Comments()
        {
            db = (SqlDatabase)EnterpriseLibraryContainer.Current.GetInstance<Database>(StringMessages.EPSConnectionString);
        }

        public IEnumerable<Entities.Comments> GetCommentsByCategory(int clientProjectId, int commentCategoryId)
        {
            var comments = new List<Entities.Comments>();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetCommentsByCategory))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                sqlParam = new SqlParameter(DBResources.param_CommentCategoryId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, commentCategoryId));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        var comment = new Entities.Comments();
                        AssignComment(reader, comment);
                        comments.Add(comment);
                    }
                }
            }

            return comments;
        }

        public Entities.Comments GetCommentsById(int clientProjectId, int id)
        {
            var comment = new Entities.Comments();

            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_GetCommentsById))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, clientProjectId));

                sqlParam = new SqlParameter(DBResources.param_Id, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, id));

                using (IDataReader reader = db.ExecuteReader(sqlCommand))
                {
                    while (reader.Read())
                    {
                        AssignComment(reader, comment);
                    }
                }
            }

            return comment;
        }

        public void InsertUpdateComments(Entities.Comments comments)
        {
            using (DbCommand sqlCommand = db.GetStoredProcCommand(DBResources.sp_InsertUpdateComments))
            {
                SqlParameter sqlParam;
                sqlParam = new SqlParameter(DBResources.param_Id, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, comments.Id));

                sqlParam = new SqlParameter(DBResources.param_ClientProjectId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, comments.ClientProjectId));

                sqlParam = new SqlParameter(DBResources.param_Description, SqlDbType.VarChar, -1);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, comments.Description));

                sqlParam = new SqlParameter(DBResources.param_CommentCategoryId, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, comments.CommentCategoryId));

                sqlParam = new SqlParameter(DBResources.param_DisplayOrder, SqlDbType.Int);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, comments.DisplayOrder));

                sqlParam = new SqlParameter(DBResources.param_IsActive, SqlDbType.Bit);
                sqlCommand.Parameters.Add(Helper.AssignSqlParameter(sqlParam, ParameterDirection.Input, ((comments.isActive == true) ? 1 : 0)));
                
                db.ExecuteNonQuery(sqlCommand);
            }
        }

        private Entities.Comments AssignComment(IDataReader reader, Entities.Comments comment)
        {
            int i;

            i = reader.GetOrdinal(DBResources.col_Id);
            if (!reader.IsDBNull(i))
                comment.Id = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_ClientProjectId);
            if (!reader.IsDBNull(i))
                comment.ClientProjectId = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_Description);
            if (!reader.IsDBNull(i))
                comment.Description = reader.GetString(i);

            i = reader.GetOrdinal(DBResources.col_CommentCategoryId);
            if (!reader.IsDBNull(i))
                comment.CommentCategoryId = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_DisplayOrder);
            if (!reader.IsDBNull(i))
                comment.DisplayOrder = reader.GetInt32(i);

            i = reader.GetOrdinal(DBResources.col_IsActive);
            if (!reader.IsDBNull(i))
                comment.isActive = reader.GetBoolean(i);

            return comment;
        }

        public void Dispose()
        {
            db = null;
        }
    }
}
