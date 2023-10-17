using MediatR;
using Dapper;

namespace FactoryAPI.Queries;

public record GetItemsQuery : IRequest<List<GetItemDto>>;

public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, List<GetItemDto>>
{
    private readonly DapperContext _context;

    public GetItemsQueryHandler(DapperContext context)
    {
        _context = context;
    }

    public async Task<List<GetItemDto>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
    {
        var query = @"SELECT * FROM Items";
        
        using var connection = _context.CreateConnection();

        var queryResult = await connection.QueryAsync<GetItemDto>(query);

        return queryResult.ToList();
    }
}