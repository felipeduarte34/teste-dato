using Microsoft.EntityFrameworkCore;

namespace TesteBackend.Persistence.Db
{
    public class CleanArchWriteDbContext : AppDbContext
    {
        public CleanArchWriteDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public CleanArchWriteDbContext() { }
    }
}