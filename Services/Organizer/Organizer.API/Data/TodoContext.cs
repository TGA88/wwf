
using Microsoft.EntityFrameworkCore;
using wwf.Services.Organizer.API.Models;

namespace wwf.Services.Organizer.API.Data
{
    public class TodoContext:DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options):base(options){

        }
        
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}