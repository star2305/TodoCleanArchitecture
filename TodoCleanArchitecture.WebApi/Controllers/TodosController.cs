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
        private readonly ILogger<TodosController> _logger;

        public TodosController(CreateTodoUseCase createTodo, GetTodosUseCase getTodos, ILogger<TodosController> logger)
        {
            _createTodo = createTodo;
            _getTodos = getTodos;
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
    }
}
