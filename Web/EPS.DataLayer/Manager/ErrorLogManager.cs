using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EPS.Interfaces;

namespace EPS.DataLayer
{
    public partial class ErrorLogManager : IDisposable, IErrorLogs
    {
        ErrorLog errorLogObject;

        public ErrorLogManager()
        {
            errorLogObject = new ErrorLog();
        }

        public void InsertErrorLog(Entities.ErrorLogs errorLog)
        {
            try
            {
                errorLogObject.InsertErrorLog(errorLog);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            errorLogObject = null;
        }

    }
}
