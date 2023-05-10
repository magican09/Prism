using PrismWorkApp.OpenWorkLib.Data;
using PrismWorkApp.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace PrismWorkApp.Services.Interfaces
{
    public interface IbldProjectRepository : IRepository<bldProject>
    {
        bldProject GetProjectWithObjects(Guid id);
        List<bldProject> GetAllAsync();
    }
}
