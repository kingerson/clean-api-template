namespace MsClean.Presentation.Extensions;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using MsClean.Application;
using Serilog;

[ExcludeFromCodeCoverage]
public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        #region Exceptions

        _ = app.UseGlobalExceptionHandler();

        #endregion

        #region Logging

        _ = app.UseSerilogRequestLogging();

        #endregion Logging

        #region Security

        _ = app.UseHsts();

        #endregion Security

        #region API Configuration

        _ = app.UseHttpsRedirection();

        #endregion API Configuration

        #region Swagger

        var ti = CultureInfo.CurrentCulture.TextInfo;

        _ = app.UseSwagger();
        _ = app.UseSwaggerUI(c =>
            c.SwaggerEndpoint(
                "/swagger/v1/swagger.json",
                $"MsClean - {ti.ToTitleCase(app.Environment.EnvironmentName)} - V1"));

        #endregion Swagger

        #region Cors
        _ = app.UseCors(builder => builder
                                   .AllowAnyHeader()
                                   .AllowAnyMethod()
                                   .AllowAnyHeader()
                                   .SetIsOriginAllowed(origin => true)
                                   .AllowCredentials()
                                   );
        #endregion

        #region Controllers
        _ = app.UseAuthorization();
        _ = app.MapControllers();
        #endregion

        #region SignalR
        _ = app.MapHub<SignalrHub>("/hub");
        #endregion

        return app;
    }
}
