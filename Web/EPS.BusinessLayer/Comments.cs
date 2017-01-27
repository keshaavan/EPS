using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EPS.DataLayer;
using EPS.Entities;
using EPS.Resources;

namespace EPS.BusinessLayer
{
    public partial class Comments : IDisposable
    {
        CommentsManager manager;

        public Comments()
        {
            manager = new CommentsManager();
        }

        public IEnumerable<Entities.Comments> GetCommentsByCategory(int clientProjectId, int commentCategoryId, Nullable<Boolean> isActive)
        {
            try
            {
                if (isActive.HasValue)
                {
                    if (isActive.Value)
                        return manager.GetCommentsByCategory(clientProjectId, commentCategoryId).Where(c => c.isActive == true);
                    else
                        return manager.GetCommentsByCategory(clientProjectId, commentCategoryId).Where(c => c.isActive == false);
                }

                return manager.GetCommentsByCategory(clientProjectId, commentCategoryId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ValidateComment(Entities.Comments comment)
        {
            try
            {
                var commentCategoryObj = this.GetCommentsByCategory(comment.ClientProjectId, comment.CommentCategoryId, null).Where(a => a.Description.ToLower() == comment.Description.ToLower() && a.Id != comment.Id).ToList();

                if (commentCategoryObj.Count > 0)
                    throw new ApplicationException(ValidationMessages.CommentDescriptionAlreadyExists);
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

        public void InsertUpdateComments(Entities.Comments comment)
        {
            try
            {
                if(comment.isActive)
                    ValidateComment(comment);

                manager.InsertUpdateComments(comment);
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

        public void Dispose()
        {
            manager = null;
        }
    }
}
