namespace MsClean.Infrastructure;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MsClean.Domain;

public class ApplicationDbContext : DbContext
{
    private readonly EntityInterceptor _entityInterceptor;
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        EntityInterceptor entityInterceptor
        ) : base(options)
    {
        _entityInterceptor = entityInterceptor ?? throw new ArgumentNullException(nameof(entityInterceptor));
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../Presentation/appsettings.json").Build();

            var databaseConnections = configuration.GetConnectionString("ConnectionEntity");
            DbContextOptionsBuilder<ApplicationDbContext> builder = new();
            builder.UseMySQL(databaseConnections)
                .EnableDetailedErrors()
                .AddInterceptors(new EntityInterceptor());
            return new ApplicationDbContext(builder.Options, new EntityInterceptor());
        }
    }

    public DbSet<Person> Persons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_entityInterceptor);
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Persona

        modelBuilder.ApplyConfiguration(new PersonConfigurarion());
        #endregion

        base.OnModelCreating(modelBuilder);
    }
}
