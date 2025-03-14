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
        ) : base(options) => _entityInterceptor = entityInterceptor ?? throw new ArgumentNullException(nameof(entityInterceptor));

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../Presentation/appsettings.json").Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            Console.WriteLine($"Connection String: {connectionString}");
            DbContextOptionsBuilder<ApplicationDbContext> builder = new();
            builder.UseSqlServer(connectionString)
                .EnableDetailedErrors()
                .AddInterceptors(new EntityInterceptor());
            return new ApplicationDbContext(builder.Options, new EntityInterceptor());
        }
    }

    public DbSet<Person> Persons { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<PermissionType> PermissionTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_entityInterceptor);
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Persona

        modelBuilder.ApplyConfiguration(new PersonConfiguration());
        #endregion

        modelBuilder.ApplyConfiguration(new PermissionConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionTypeConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
