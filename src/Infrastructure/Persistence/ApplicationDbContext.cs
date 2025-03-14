namespace MsClean.Infrastructure;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MsClean.Domain;

public class ApplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    private readonly EntityInterceptor _entityInterceptor;
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IConfiguration configuration,
        EntityInterceptor entityInterceptor
        ) : base(options)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _entityInterceptor = entityInterceptor ?? throw new ArgumentNullException(nameof(entityInterceptor));
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configurationPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Presentation", "appsettings.json");

            var configuration = new ConfigurationBuilder()
                                    .SetBasePath(Path.GetDirectoryName(configurationPath)!).AddJsonFile(configurationPath).Build();
                                    
            // var configuration = new ConfigurationBuilder()
            //     .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../Presentation/appsettings.json").Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            DbContextOptionsBuilder<ApplicationDbContext> builder = new();
            builder.UseSqlServer(connectionString)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .AddInterceptors(new EntityInterceptor());
            return new ApplicationDbContext(builder.Options, configuration, new EntityInterceptor());
        }
    }

    public DbSet<Person> Persons { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<PermissionType> PermissionTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseSqlServer(connectionString)
                         .EnableDetailedErrors()
                         .EnableSensitiveDataLogging()
                         .AddInterceptors(_entityInterceptor);
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
