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

        public TodosController(CreateTodoUseCase createTodo, GetTodosUseCase getTodos)
        {
            _createTodo = createTodo;
            _getTodos = getTodos;
        }

        // POST: api/todos
        [HttpPost]
        public async Task<ActionResult<TodoResponse>> Create([FromBody] CreateTodoRequest request)
        {
            var result = await _createTodo.ExecuteAsync(request);
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
