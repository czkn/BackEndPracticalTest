using Dapper;

namespace FactoryAPI;

public class ItemExistenceChecker
{
    private readonly DapperContext _context;

    public ItemExistenceChecker(DapperContext context)
    {
        _context = context;
    }
    
    public async Task<bool> CheckIfItemExists(Guid itemId)
    {
        using var connection = _context.CreateConnection();
        
        var query = "SELECT COUNT(*) FROM Items WHERE Id = @Id";
        var itemExists = await connection.ExecuteScalarAsync<int>(query, new { Id = itemId });

        return itemExists > 0;
    }
}