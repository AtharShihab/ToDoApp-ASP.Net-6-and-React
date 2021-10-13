using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.Models.ToDos.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDosController : ControllerBase
    {
        private readonly ToDoContext _toDoContext;

        public ToDosController(ToDoContext toDoContext)
        {
            _toDoContext = toDoContext;
        }

        // GET: api/<ToDosController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetTodos()
        {
            return await _toDoContext.ToDos.ToListAsync();
        }

        // GET api/<ToDosController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDo>> GetTodo(int id)
        {
            var todo = await _toDoContext.ToDos.FindAsync(id);
            if(todo == null)
            {
                return NotFound();
            }
            return todo;
        }

        // POST api/<ToDosController>
        [HttpPost]
        public async Task<ActionResult<ToDo>> CreateTodo(ToDo toDo)
        {
            _toDoContext.Add(toDo);
            await _toDoContext.SaveChangesAsync();

            return CreatedAtAction("GetTodo", new { id = toDo.Id}, toDo);
        }

        // PUT api/<ToDosController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ToDo>> UpdateTodo(int id, ToDo todo)
        {
            if(id != todo.Id)
            {
                return BadRequest();
            }

            _toDoContext.Entry(todo).State = EntityState.Modified;

            try
            {
                await _toDoContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return todo;
        }

        // DELETE api/<ToDosController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var todo = await _toDoContext.ToDos.FindAsync(id);
            if(todo == null)
            {
                return NotFound();
            }

            _toDoContext.ToDos.Remove(todo);
            await _toDoContext.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoExists(int id)
        {
            return _toDoContext.ToDos.Any(d => d.Id == id);
        }
    }
}
