using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoCleanArchitecture.Domain.Entites;

namespace TodoCleanArchitecture.Infrastructure.Persistence
{
    public class TodoDbContext: DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }

        public DbSet<TodoItem> Todos => Set<TodoItem>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TodoItem 테이블 설정(최소)
            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.ToTable("Todos");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(x => x.CreatedAt)
                      .IsRequired();
            });
        }
    }
}
