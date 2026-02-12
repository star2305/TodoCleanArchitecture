using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoCleanArchitecture.Application.Interfaces;
using TodoCleanArchitecture.Infrastructure.Persistence;

namespace TodoCleanArchitecture.Infrastructure.Logging
{
    public class DbAuditLogWriter : IAuditLogWriter
    {
        private readonly TodoDbContext _dbContext;

        public DbAuditLogWriter(TodoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task WriteAsync(string level, string category, string action, string message, string? username = null, string? traceId = null, string? dataJson = null, string? exception = null)
        {
            var logEntry = new Domain.Entities.AuditLog(
                level: level,
                category: category,
                action: action,
                message: message,
                username: username,
                traceId: traceId,
                dataJson: dataJson,
                exception: exception
            );

            _dbContext.AuditLogs.Add(logEntry);
            await _dbContext.SaveChangesAsync();
        }
    }
}
