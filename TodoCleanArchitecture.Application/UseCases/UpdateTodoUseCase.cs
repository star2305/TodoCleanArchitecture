using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoCleanArchitecture.Application.Interfaces;

namespace TodoCleanArchitecture.Application.UseCases
{
    public class UpdateTodoUseCase
    {
        private readonly ITodoRepository _repo;

        public UpdateTodoUseCase(ITodoRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> ExecuteAsync(int id)
        {
            var todo = await _repo.GetByIdAsync(id);

            if (todo == null) return false;

            // Business logic to update the todo item
            todo.Complete();

            await _repo.UpdateAsync(todo);
            return true;
        }
    }
}
