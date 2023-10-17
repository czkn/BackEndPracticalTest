using FactoryAPI.Commands;
using FactoryAPI.Queries;
using Microsoft.AspNetCore.Mvc;

namespace FactoryAPI.Controllers
{
    [ApiController]
    public class ItemsController : ApiControllerBase
    {
        // Create a new item
        [HttpPost]
        public async Task<ActionResult> CreateItem(CreateItemCommand createItemCommand)
        {
            var commandResult = await Mediator.Send(createItemCommand);
            
            if (commandResult is string)
                return BadRequest(commandResult);
            
            return Created("", createItemCommand);
        }
        
        // Retrieve a single item
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult> GetItem(Guid id)
        {
            var queryResult = await Mediator.Send(new GetItemQuery { Id = id });
            
            if (queryResult is false)
                return NotFound("Item with given Id does not exist");

            return Ok(queryResult);
        }

        // Retrieve all items (only if you'll be using an ORM framework)
        [HttpGet]
        public async Task<List<GetItemDto>> GetItems() => await Mediator.Send(new GetItemsQuery());

        // Update an existing item
        [HttpPut]
        public async Task<ActionResult> EditItem(EditItemCommand editItemCommand)
        {
            var commandResult = await Mediator.Send(new EditItemCommand()
            {
                Id = editItemCommand.Id, Name = editItemCommand.Name, Description = editItemCommand.Description,
                Price = editItemCommand.Price
            });

            if(commandResult is string)
                return BadRequest(commandResult);
            
            if (commandResult is false)
                return NotFound("Item with given Id does not exist");

            return NoContent();
        }

        // Delete an item
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteItem(Guid id)
        {
            var commandResult = await Mediator.Send(new DeleteItemCommand { Id = id });
            
            if(!commandResult)
                return NotFound("Item with given Id does not exist");

            return NoContent();
        }
            
    }
}