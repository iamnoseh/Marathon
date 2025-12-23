using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Extensions;

public static class MigrationExtensions
{
    public static async Task<IApplicationBuilder> UseDatabaseMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        await dbContext.Database.MigrateAsync();
        
        return app;
    }
}
