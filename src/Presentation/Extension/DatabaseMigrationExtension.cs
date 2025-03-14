namespace MsClean.Presentation.Extensions;

using Microsoft.EntityFrameworkCore;
using MsClean.Infrastructure;

public static class DatabaseMigrationExtension
{
    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated(); 
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al aplicar migraciones: {ex.Message}");
            }
        }

        return app;
    }
}