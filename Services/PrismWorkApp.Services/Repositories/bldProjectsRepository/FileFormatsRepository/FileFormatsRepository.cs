using Microsoft.EntityFrameworkCore;
using PrismWorkApp.OpenWorkLib.Data;
using System.Collections.Generic;
using System.Linq;

namespace PrismWorkApp.Services.Repositories
{
    public class FileFormatsRepository : Repository<FileFormat>
    {
        public FileFormatsRepository(DbContext context) : base(context)
        {

        }
        public void Dispose()
        {
            this.Dispose();
        }
        public List<FileFormat> GetAllFileFormats()//(Guid id)
        {
            return PlutoContext.FileFormats.ToList();//out_val;
        }

        public bldProjectsPlutoContext PlutoContext { get { return Context as bldProjectsPlutoContext; } }
    }
}
