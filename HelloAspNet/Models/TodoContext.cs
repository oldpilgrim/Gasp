using Microsoft.EntityFrameworkCore;

namespace HelloAspNet.Models
{
    // The database context is the main class that coordinates Entity Framework functionality for a data model.
    
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}