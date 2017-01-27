using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPS.Interfaces
{
    public interface IErrorLogs
    {
        void InsertErrorLog(Entities.ErrorLogs errorLog);
    }
}
