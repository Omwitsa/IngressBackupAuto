using IngressBkpAutomation.Models;
using Microsoft.EntityFrameworkCore;

namespace IngressBkpAutomation.Utilities
{
    public class Utility
    {
        public static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<IngressSetupDbContext>();
                context.Database.Migrate();
                context.EnsureDatabaseSeeded();
                // context.Database.EnsureCreated();
            }
        }
    }
}
