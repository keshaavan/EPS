using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EPS.Interfaces;
using EPS.Entities;

namespace EPS.DataLayer
{
    public partial class CommentsManager : IComments, IDisposable
    {
        Comments commentsObject;

        public CommentsManager()
        {
            commentsObject = new Comments();
        }

        public IEnumerable<Entities.Comments> GetCommentsByCategory(int clientProjectId, int commentCategoryId)
        {
            try
            {
                var comments = commentsObject.GetCommentsByCategory(clientProjectId, commentCategoryId);
                var clientProject = new ClientProjectManager().GetClientProjectById(clientProjectId);

                foreach (var comment in comments)
                {
                    var lookupManager = new LookupManager();
                    comment.CommentCategory = lookupManager.GetLookupById(clientProjectId, comment.CommentCategoryId);
                    comment.ClientProject = clientProject;
                }

                return comments;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Entities.Comments GetCommentsById(int clientProjectId, int id)
        {
            try
            {
                var comment = commentsObject.GetCommentsById(clientProjectId, id);
                var clientProject = new ClientProjectManager();
                var lookupManager = new LookupManager();

                comment.CommentCategory = lookupManager.GetLookupById(clientProjectId, comment.CommentCategoryId);
                comment.ClientProject = clientProject.GetClientProjectById(clientProjectId);

                return comment;
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
                commentsObject.InsertUpdateComments(comment);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            commentsObject = null;
        }
    }
}
