using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPS.Interfaces
{
    public interface IComments
    {
        IEnumerable<Entities.Comments> GetCommentsByCategory(int clientProjectId, int commentCategoryId);
        Entities.Comments GetCommentsById(int clientProjectId, int id);
        void InsertUpdateComments(Entities.Comments comment);
    }
}
