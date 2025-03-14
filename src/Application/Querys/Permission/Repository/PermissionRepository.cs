namespace MsClean.Application;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

public class PermissionRepository : IPermissionRepository
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    public PermissionRepository(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _connectionString = _configuration["ConnectionStrings:DefaultConnection"];
    }

    public async Task<IEnumerable<PermissionViewModel>> GetAll(CancellationToken cancellationToken)
    {
        IEnumerable<PermissionViewModel> result;

        var query = @"SELECT  [p].[Id], 
                               [p].[EmployeeForename],
                               [p].[EmployeeLastName],
                               [p].[PermissionDate],
                               [p].[PermissionTypeId],
                               [p].[UserRegister],
                               [p].[DateTimeRegister],
                               [p].[IsActive],
                               [pt].[Description] AS PermissionType
                        FROM [dbo].[Permission] [p]
                        INNER JOIN [dbo].[PermissionType] [pt] ON [p].[PermissionTypeId] = [pt].[Id]";

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            result = await connection.QueryAsync<PermissionViewModel>(query, commandType: CommandType.Text);
        }

        return result;
    }
    public async Task<PermissionViewModel> GetById(int permissionId,CancellationToken cancellationToken)
    {
       PermissionViewModel result;

        var query = @"SELECT   [p].[Id], 
                               [p].[EmployeeForename],
                               [p].[EmployeeLastName],
                               [p].[PermissionDate],
                               [p].[PermissionTypeId],
                               [p].[UserRegister],
                               [p].[DateTimeRegister],
                               [p].[IsActive],
                               [pt].[Description] AS PermissionType
                        FROM [dbo].[Permission] [p]
                        INNER JOIN [dbo].[PermissionType] [pt] ON [p].[PermissionTypeId] = [pt].[Id]
                        WHERE [p].[Id] = @PermissionId";

        var parameters = new { PermissionId = permissionId };

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            result = await connection.QueryFirstOrDefaultAsync<PermissionViewModel>(query, parameters , commandType: CommandType.Text);
        }

        return result;
    }
}
