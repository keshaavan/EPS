using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EPS.Entities;
using EPS.DataLayer;

namespace EPS.BusinessLayer
{
    public partial class ErrorLog : IDisposable
    {
        ErrorLogManager manager;

        public ErrorLog()
        {
            manager = new ErrorLogManager();
        }

        public void InsertErrorLog(Entities.ErrorLogs errorLog)
        {
            try
            {
                manager.InsertErrorLog(errorLog);
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
