using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoCleanArchitecture.Application.Interfaces;
using TodoCleanArchitecture.Domain.Entites;
using TodoCleanArchitecture.Infrastructure.Persistence;

namespace TodoCleanArchitecture.Infrastructure.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoDbContext _db;
        
        public TodoRepository(TodoDbContext db)
        {
            _db = db;
        }

        public async Task<TodoItem> AddAsync(TodoItem item)
        {
            _db.Todos.Add(item);
            await _db.SaveChangesAsync();
            return item;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _db.Todos.FirstOrDefaultAsync(x => x.Id == id);
            if (item != null)
            {
                _db.Todos.Remove(item);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<TodoItem>> GetAllAsync()
        {
            return await _db.Todos
                .OrderByDescending(t => t.Id)
                .ToListAsync();
        }

        public Task<TodoItem?> GetByIdAsync(int id)
        {
            return _db.Todos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(TodoItem item)
        {
            _db.Todos.Update(item);
            await _db.SaveChangesAsync();
        }
    }
}
