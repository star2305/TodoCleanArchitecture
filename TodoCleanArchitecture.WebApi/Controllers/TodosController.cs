using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoCleanArchitecture.Application.Dtos;
using TodoCleanArchitecture.Application.UseCases;

namespace TodoCleanArchitecture.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TodosController : ControllerBase
    {
        private readonly CreateTodoUseCase _createTodo;
        private readonly GetTodosUseCase _getTodos;
        private readonly UpdateTodoUseCase _updateTodo;
        private readonly DeleteTodoUseCase _deleteTodo;
        private readonly ILogger<TodosController> _logger;

        public TodosController(CreateTodoUseCase createTodo, GetTodosUseCase getTodos, UpdateTodoUseCase updateTodo, DeleteTodoUseCase deleteTodo, ILogger<TodosController> logger)
        {
            _createTodo = createTodo;
            _getTodos = getTodos;
            _updateTodo = updateTodo;
            _deleteTodo = deleteTodo;
            _logger = logger;
        }

        // POST: api/todos
        [HttpPost]
        public async Task<ActionResult<TodoResponse>> Create([FromBody] CreateTodoRequest request)
        {
            _logger.LogInformation("Create Todo requested by user: {User}", User.Identity?.Name);

            var result = await _createTodo.ExecuteAsync(request);

            _logger.LogInformation("Todo created with Id {Id}", result.Id);

            return Ok(result);
        }

        // GET: api/todos
        [HttpGet]
        public async Task<ActionResult<List<TodoResponse>>> GetAll()
        {
            var result = await _getTodos.ExecuteAsync();
            return Ok(result);
        }

        // PUT: api/todos/{id}/complete
        [HttpPut("{id:int}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            var ok = await _updateTodo.ExecuteAsync(id);
            if (!ok) return NotFound(new { message = "Todo not found." });

            return NoContent(); // 204
        }

        // DELETE: api/todos/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _deleteTodo.ExecuteAsync(id);
            if (!ok) return NotFound(new { message = "Todo not found." });

            return NoContent(); // 204
        }
    }
}
