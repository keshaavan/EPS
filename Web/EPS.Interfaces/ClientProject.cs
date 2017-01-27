using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPS.Entities;

namespace EPS.Interfaces
{
    public interface IClientProject
    {
        IEnumerable<Entities.ClientProject> GetAllClientProjects();
        Entities.ClientProject GetClientProjectById(int id);
    }
}
