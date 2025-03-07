namespace MsClean.Application;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

public class PersonQueryRepository : IPersonQueryRepository
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    public PersonQueryRepository(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _connectionString = _configuration["ConnectionStrings:ConnectionEntity"];
    }

    public async Task<IEnumerable<PersonViewModel>> GetAll(CancellationToken cancellationToken)
    {
        IEnumerable<PersonViewModel> result;

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            result = await connection.QueryAsync<PersonViewModel>("sp_get_person", commandType: CommandType.StoredProcedure);
        }

        return result;
    }

    public async Task<PersonViewModel> GetById(GetPersonByIdQuery request, CancellationToken cancellationToken)
    {
        PersonViewModel result;

        var parameters = new DynamicParameters();
        parameters.Add("@Id", request.Id);

        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);
            result = await connection.QueryFirstOrDefaultAsync<PersonViewModel>("sp_get_person_by_id", parameters, commandType: CommandType.StoredProcedure);
        }

        return result;
    }

}
