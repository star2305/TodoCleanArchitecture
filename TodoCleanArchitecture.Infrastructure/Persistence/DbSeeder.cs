using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoCleanArchitecture.Domain.Entities;
using TodoCleanArchitecture.Infrastructure.Security;

namespace TodoCleanArchitecture.Infrastructure.Persistence
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(TodoDbContext db)
        {
            await db.Database.MigrateAsync();

            if (!await db.Users.AnyAsync())
            {
                var (hash, salt) = PasswordHasher.HashPassword("1234");
                db.Users.Add(new User("admin", hash, salt, role: "Admin"));
                await db.SaveChangesAsync();
            }
        }
    }
}
