using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoCleanArchitecture.Domain.Entities;

namespace TodoCleanArchitecture.Infrastructure.Persistence
{
    public class TodoDbContext: DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }

        public DbSet<TodoItem> Todos => Set<TodoItem>();
        public DbSet<User> Users => Set<User>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

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

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(x => x.Id);

                entity.HasIndex(x => x.Username).IsUnique();

                entity.Property(x => x.Username).IsRequired().HasMaxLength(100);
                entity.Property(x => x.PasswordHash).IsRequired().HasMaxLength(500);
                entity.Property(x => x.PasswordSalt).IsRequired().HasMaxLength(500);
                entity.Property(x => x.Role).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.ToTable("AuditLogs");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.CreatedAtUtc).IsRequired();

                entity.Property(x => x.Level).IsRequired().HasMaxLength(20);
                entity.Property(x => x.Category).IsRequired().HasMaxLength(50);
                entity.Property(x => x.Action).IsRequired().HasMaxLength(80);

                entity.Property(x => x.Username).HasMaxLength(100);
                entity.Property(x => x.TraceId).HasMaxLength(100);

                entity.Property(x => x.Message).IsRequired().HasMaxLength(2000);

                entity.Property(x => x.DataJson).HasColumnType("nvarchar(max)");
                entity.Property(x => x.Exception).HasColumnType("nvarchar(max)");

                entity.HasIndex(x => x.CreatedAtUtc);
                entity.HasIndex(x => x.Category);
                entity.HasIndex(x => x.Level);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshTokens");
                entity.HasKey(x => x.Id);

                entity.Property(x => x.TokenHash).IsRequired().HasMaxLength(500);
                entity.Property(x => x.ExpiresAtUtc).IsRequired();
                entity.Property(x => x.CreatedAtUtc).IsRequired();

                entity.Property(x => x.CreatedByIp).HasMaxLength(60);
                entity.Property(x => x.RevokedByIp).HasMaxLength(60);

                entity.HasIndex(x => x.UserId);
                entity.HasIndex(x => x.TokenHash).IsUnique();
            });
        }
    }
}
