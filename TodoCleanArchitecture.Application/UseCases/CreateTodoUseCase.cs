using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoCleanArchitecture.Application.Dtos;
using TodoCleanArchitecture.Application.Interfaces;
using TodoCleanArchitecture.Domain.Entites;

namespace TodoCleanArchitecture.Application.UseCases
{
    public class CreateTodoUseCase
    {
        private readonly ITodoRepository _repo;

        public CreateTodoUseCase(ITodoRepository repo)
        {
            _repo = repo;
        }

        public async Task<TodoResponse> ExecuteAsync(CreateTodoRequest request)
        {
            var todo = new TodoItem(request.Title);
            var saved = await _repo.AddAsync(todo);

            return new TodoResponse
            {
                Id = saved.Id,
                Title = saved.Title,
                IsCompleted = saved.IsCompleted,
                CreatedAt = saved.CreatedAt
            };
        }
    }
}
