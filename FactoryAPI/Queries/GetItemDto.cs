namespace FactoryAPI.Queries;

public class GetItemDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }
}