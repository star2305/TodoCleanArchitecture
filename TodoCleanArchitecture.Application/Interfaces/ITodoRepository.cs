using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoCleanArchitecture.Domain.Entites;

namespace TodoCleanArchitecture.Application.Interfaces
{
    public interface ITodoRepository
    {
        Task<TodoItem> AddAsync(TodoItem item);
        Task<List<TodoItem>> GetAllAsync();
        Task<TodoItem?> GetByIdAsync(int id);
        Task UpdateAsync(TodoItem item);
        Task DeleteAsync(int id);
    }
}
