using Microsoft.EntityFrameworkCore;
using MsClean.Infrastructure;
using MsClean.Presentation.Extensions;

var builder = WebApplication
    .CreateBuilder(args)
    .ConfigureApplicationBuilder();

var app = builder
    .Build()
    .ConfigureApplication()
    .ApplyMigrations();

// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     try
//     {
//         var config = services.GetRequiredService<IConfiguration>(); // 🔹 Se obtiene desde 'scope.ServiceProvider'
//         Console.WriteLine($"Connection String: {config.GetConnectionString("DefaultConnection")}");

//         var context = services.GetRequiredService<ApplicationDbContext>(); // 🔹 Se obtiene desde 'services'
//         await context.Database.EnsureCreatedAsync(); 
//         await context.Database.MigrateAsync();
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine($"Error al aplicar migraciones: {ex.Message}");
//     }
// }

await app.RunAsync();

