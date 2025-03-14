using MsClean.Presentation.Extensions;

var builder = WebApplication
    .CreateBuilder(args)
    .ConfigureApplicationBuilder();

var app = builder
    .Build()
    .ConfigureApplication();
    // .ApplyMigrations();

await app.RunAsync();

