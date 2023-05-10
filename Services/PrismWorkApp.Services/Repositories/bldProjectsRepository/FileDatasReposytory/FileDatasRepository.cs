using PrismWorkApp.OpenWorkLib.Data;

namespace PrismWorkApp.Services.Repositories
{
    public class FileDatasRepository : Repository<FileData>, IFileDatasRepository
    {
       
        public FileDatasRepository(bldProjectsPlutoContext context) : base(context)
        {

        }
        public void Dispose()
        {
            this.Dispose();
        }

        
    }
}
