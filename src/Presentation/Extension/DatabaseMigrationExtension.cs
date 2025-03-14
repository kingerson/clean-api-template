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
                var config = services.GetRequiredService<IConfiguration>(); // 🔹 Se obtiene desde 'scope.ServiceProvider'
                Console.WriteLine($"Connection String: {config.GetConnectionString("DefaultConnection")}");

                var context = services.GetRequiredService<ApplicationDbContext>(); // 🔹 Se obtiene desde 'services'
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