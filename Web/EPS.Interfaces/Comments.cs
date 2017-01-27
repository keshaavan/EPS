using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPS.Interfaces
{
    public interface IComments
    {
        IEnumerable<Entities.Comments> GetCommentsByCategory(string client, int commentCategoryId);
        Entities.Comments GetCommentsById(string client, int id);
        void InsertUpdateComments(Entities.Comments comment);
    }
}
