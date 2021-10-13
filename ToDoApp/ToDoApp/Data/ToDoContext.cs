using Microsoft.EntityFrameworkCore;
using ToDoApp.Models.ToDos.Entities;

namespace ToDoApp.Data
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options) { }

        public DbSet<ToDo> ToDos {  get; set; }
    }
}
