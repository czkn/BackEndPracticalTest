using Dapper;
using FluentValidation;
using MediatR;

namespace FactoryAPI.Commands;

public class DeleteItemCommand: IRequest<bool>
{
    public Guid Id { get; set; }
}

public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, bool>
{
    private readonly DapperContext _context;
    private readonly ItemExistenceChecker _checker;

    public DeleteItemCommandHandler(DapperContext context, ItemExistenceChecker checker)
    {
        _context = context;
        _checker = checker;
    }


    public async Task<bool> Handle(DeleteItemCommand command, CancellationToken cancellationToken)
    {
        var itemExists = await _checker.CheckIfItemExists(command.Id);
        
        if (!itemExists)
            return false;
        
        var query = "DELETE FROM Items WHERE Id = @Id";

        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(query, new
        {
            Id = command.Id
        });

        return true;
    }
}
    