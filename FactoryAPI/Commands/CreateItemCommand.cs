using Dapper;
using FluentValidation;
using MediatR;

namespace FactoryAPI.Commands;

public class CreateItemCommand : IRequest<object>
{
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public decimal Price { get; set; }
}

public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, object>
{
    private readonly DapperContext _context;
    private readonly IValidator<CreateItemCommand> _validator;

    public CreateItemCommandHandler(DapperContext context, IValidator<CreateItemCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<object> Handle(CreateItemCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await _validator.ValidateAndThrowAsync(command, cancellationToken);
        }
        catch (ValidationException ex)
        {
            return ex.Message;
        }

        var query = @"INSERT INTO Items (Id, Name, Description, Price) VALUES (@Id, @Name, @Description, @Price)";

        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(query, new
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
        });

        return Unit.Value;
    }
}