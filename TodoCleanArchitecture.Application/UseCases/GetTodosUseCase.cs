using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoCleanArchitecture.Application.Dtos;
using TodoCleanArchitecture.Application.Interfaces;

namespace TodoCleanArchitecture.Application.UseCases
{
    public class GetTodosUseCase
    {
        private readonly ITodoRepository _repo;

        public GetTodosUseCase(ITodoRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<TodoResponse>> ExecuteAsync()
        {
            var items = await _repo.GetAllAsync();
            return items.Select(t => new TodoResponse
            {
                Id = t.Id,
                Title = t.Title,
                IsCompleted = t.IsCompleted,
                CreatedAt = t.CreatedAt
            }).ToList();
        }
    }
}
