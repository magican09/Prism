using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrismWorkApp.Services.Repositories 
{
   public  class bldMaterialsRepository:Repository<bldMaterial>
    {
        public bldMaterialsRepository(PlutoContext context) : base(context)
        {

        }
        public void Dispose()
        {
            this.Dispose();
        }
        public List<bldMaterial> GetAllAsync()
        {
            return PlutoContext.Materials.ToList();
        }

        public PlutoContext PlutoContext { get { return Context as PlutoContext; } }

    }
}
