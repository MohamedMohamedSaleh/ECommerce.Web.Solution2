using ECommerce.Domain.Contracts;
using ECommerce.persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ECommerce.Web.Extensions
{
    public static class WebAppRegisteration
    {
        public static async Task<WebApplication> MigrateDbAsync(this WebApplication app)
        {
            await using var Scope = app.Services.CreateAsyncScope();
            var dbcontextService = Scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            var PendingMigrations = await dbcontextService!.Database.GetAppliedMigrationsAsync();
            
            if (dbcontextService != null)
            {
                if (PendingMigrations.Any())
                   await dbcontextService.Database.MigrateAsync();
            }
            return app;

        }

        public static async Task<WebApplication> seedDbAsync(this WebApplication app)
        {
           await using var Scope = app.Services.CreateAsyncScope();

            var DataInitilizerService = Scope.ServiceProvider.GetRequiredService<IDataInitializer>();
           await DataInitilizerService.InitializeAsync();
            return app;
        }

    }
}
