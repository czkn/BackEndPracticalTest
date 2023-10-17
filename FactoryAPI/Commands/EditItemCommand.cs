using Dapper;
using FluentValidation;
using MediatR;

namespace FactoryAPI.Commands;

public class EditItemCommand : IRequest<object>
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public decimal Price { get; set; }
}

public class EditItemCommandHandler : IRequestHandler<EditItemCommand, object>
{
    private readonly DapperContext _context;
    private readonly IValidator<EditItemCommand> _validator;
    private readonly ItemExistenceChecker _checker;

    public EditItemCommandHandler(DapperContext context, IValidator<EditItemCommand> validator, ItemExistenceChecker checker)
    {
        _context = context;
        _validator = validator;
        _checker = checker;
    }


    public async Task<object> Handle(EditItemCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(command, cancellationToken);
        }
        catch (ValidationException ex)
        {
            return ex.Message;
        }
        
        var itemExists = await _checker.CheckIfItemExists(command.Id);

        if (!itemExists)
            return false;

        var query = @"UPDATE Items 
                      SET Name = @Name, Description = @Description, Price = @Price
                      WHERE Id = @Id";

        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(query, new
        {
            Id = command.Id,
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
        });

        return true;
    }
}