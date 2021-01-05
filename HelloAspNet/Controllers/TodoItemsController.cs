using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelloAspNet.Models;

namespace HelloAspNet.Controllers
{
    // https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-5.0&tabs=visual-studio-code
    
    // This attribute indicates that the controller responds to web API requests
    [ApiController]
    // The URL path for each method is constructed as follows:
    // Start with the template string in the controller's Route attribute.
    // Replace [controller] with the name of the controller, which by convention is the controller
    // class name minus the "Controller" suffix.
    // If the [HttpGet] attribute has a route template (for example, [HttpGet("products")]),
    // append that to the path.
    [Route("api/[controller]")]
    // A web API consists of one or more controller classes that derive from ControllerBase.
    // Don't create a web API controller by deriving from the Controller class.
    // Controller derives from ControllerBase and adds support for views, so it's for handling web pages,
    // not web API requests.
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        // Uses DI to inject the database context (TodoContext) into the controller.
        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            return await _context.TodoItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        // GET: api/TodoItems/5
        // "{id}" is a placeholder variable for the unique identifier of the to-do item.
        // When GetTodoItem is invoked, the value of "{id}" in the URL is provided to the method in
        // its id parameter.
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            // ASP.NET Core automatically serializes the object to JSON and writes the JSON into the body of the response message
            return ItemToDTO(todoItem);
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            
            todoItem.Name = todoItemDTO.Name;
            todoItem.IsComplete = todoItemDTO.IsComplete;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> CreateTodoItem(TodoItemDTO todoItemDTO)
        {
            var todoItem = new TodoItem
            {
                IsComplete = todoItemDTO.IsComplete,
                Name = todoItemDTO.Name
            };
            
            // The method gets the value of the to-do item from the body of the HTTP request.
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            // The CreatedAtAction method:
            // Returns an HTTP 201 status code if successful.
            // Adds a Location header to the response. The Location header specifies the URI of the newly created to-do item.
            // References the GetTodoItem action to create the Location header's URI.
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, ItemToDTO(todoItem));
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id) => _context.TodoItems.Any(e => e.Id == id);

        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
            new TodoItemDTO
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };
    }
}
