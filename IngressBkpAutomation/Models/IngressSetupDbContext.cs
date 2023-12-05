using Microsoft.EntityFrameworkCore;

namespace IngressBkpAutomation.Models
{
    public partial class IngressSetupDbContext : DbContext
    {
        public IngressSetupDbContext()
        {
        }

        public IngressSetupDbContext(DbContextOptions<IngressSetupDbContext> options)
        : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<SysSetup> SysSetup { get; set; }
    }
}
