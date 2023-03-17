using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class bldMaterialsRepository : Repository<bldMaterial>
    {
        public bldMaterialsRepository(DbContext context) : base(context)
        {

        }
        public void Dispose()
        {
            this.Dispose();
        }
        public List<bldMaterial> GetByName(string name)
        {
            return  PlutoContext.Materials.Where(m => m.Name == name).ToList();
        }
        public List<bldMaterial> GetAllAsync()
        {
            return  PlutoContext.Materials.ToList();
        }

        public bldMaterialsPlutoContext  PlutoContext { get { return Context as bldMaterialsPlutoContext; } }

    }
}
