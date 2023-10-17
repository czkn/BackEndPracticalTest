using System.Reflection.Metadata;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FactoryAPI.Queries;

public record GetItemQuery : IRequest<object>
{
    public Guid Id { get; set; }
}

public class GetItemQueryHandler : IRequestHandler<GetItemQuery, object>
{
    private readonly DapperContext _context;
    private readonly ItemExistenceChecker _checker;
    
    public GetItemQueryHandler(DapperContext context, ItemExistenceChecker checker)
    {
        _context = context;
        _checker = checker;
    }

    public async Task<object> Handle(GetItemQuery request, CancellationToken cancellationToken)
    {
        var itemExists = await _checker.CheckIfItemExists(request.Id);

        if (!itemExists)
            return false;
        
        var query = @"SELECT * FROM Items WHERE Id = @Id";
        
        using var connection = _context.CreateConnection();

        var queryResult = await connection.QuerySingleOrDefaultAsync<GetItemDto>(query, new
        {
            Id = request.Id
        });

        return queryResult;
    }
}