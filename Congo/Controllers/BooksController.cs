using System.Collections.Generic;
using Congo.Models;
using Congo.Services;
using Microsoft.AspNetCore.Mvc;

namespace Congo.Controllers
{
    // The web API controller:

    // Uses the BookService class to perform CRUD operations.
    // Contains action methods to support GET, POST, PUT, and DELETE HTTP requests.
    // Calls CreatedAtRoute in the Create action method to return an HTTP 201 response.
    // Status code 201 is the standard response for an HTTP POST method that creates a
    // new resource on the server. CreatedAtRoute also adds a Location header to the response.
    // The Location header specifies the URI of the newly created book.
    
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }
        
        [HttpGet]
        public ActionResult<List<Book>> Get() => _bookService.Get();
        
        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public ActionResult<Book> Get(string id)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }
        
        [HttpPost]
        public ActionResult<Book> Create(Book book)
        {
            _bookService.Create(book);

            return CreatedAtRoute("GetBook", new { id = book.Id.ToString() }, book);
        }
        
        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Book bookIn)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _bookService.Update(id, bookIn);

            return NoContent();
        }
        
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _bookService.Remove(book.Id);

            return NoContent();
        }
    }
}