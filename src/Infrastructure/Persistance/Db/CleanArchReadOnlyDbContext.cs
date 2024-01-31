using Microsoft.EntityFrameworkCore;

namespace TesteBackend.Persistence.Db
{
    public class CleanArchReadOnlyDbContext : AppDbContext
    {
        public CleanArchReadOnlyDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
