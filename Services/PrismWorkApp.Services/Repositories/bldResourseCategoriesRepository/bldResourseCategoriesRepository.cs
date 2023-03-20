using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrismWorkApp.Services.Repositories
{
    public class bldResourseCategoriesRepository : Repository<bldResourseCategory>
    {
        public bldResourseCategoriesRepository(DbContext context) : base(context)
        {

        }
        public void Dispose()
        {
            this.Dispose();
        }
        public List<bldMaterial> GetByName(string name)
        {
            return PlutoContext.Materials.Where(m => m.Name == name).ToList();
        }
        public List<bldResourseCategory> GetAllAsync()
        {
            return PlutoContext.ResourseCategories.ToList();
        }

        public bldMaterialsPlutoContext PlutoContext { get { return Context as bldMaterialsPlutoContext; } }

    }
}
