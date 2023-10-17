using System.Data;
using Microsoft.Data.SqlClient;

namespace FactoryAPI;

public class DapperContext
{
    private readonly string? _connectionString;


    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("FactoryDbConnectionString");
    }

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}