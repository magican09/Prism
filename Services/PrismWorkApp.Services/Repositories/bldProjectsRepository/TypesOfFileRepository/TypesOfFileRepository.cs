using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class TypesOfFileRepository : Repository<TypeOfFile>
    {
        public TypesOfFileRepository(DbContext context) : base(context)
        {

        }
        public void Dispose()
        {
            this.Dispose();
        }
        //public List<FileType> GetAllFileFormats()//(Guid id)
        //{
        //    return PlutoContext.FileTypes.ToList();//out_val;
        //}

        public bldProjectsPlutoContext PlutoContext { get { return Context as bldProjectsPlutoContext; } }
    }
}
