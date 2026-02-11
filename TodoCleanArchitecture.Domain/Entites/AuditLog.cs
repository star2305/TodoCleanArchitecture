using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoCleanArchitecture.Domain.Entites
{
    public class AuditLog
    {
        public long Id { get; private set; }

        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

        public string Level { get; private set; } = "Info"; // Info, Warning, Error
        public string Category { get; private set; } = "General"; // Auth, Todo, Exception 등
        public string Action { get; private set; } = ""; // Login, CreateTodo, Exception 등

        public string? Username { get; private set; }
        public string? TraceId { get; private set; }

        public string Message { get; private set; } = "";
        public string? DataJson { get; private set; } // 추가 데이터(JSON)
        public string? Exception { get; private set; } // 에러일 때 stacktrace 등

        protected AuditLog() { }

        public AuditLog(
            string level,
            string category,
            string action,
            string message,
            string? username = null,
            string? traceId = null,
            string? dataJson = null,
            string? exception = null)
        {
            Level = string.IsNullOrWhiteSpace(level) ? "Info" : level;
            Category = string.IsNullOrWhiteSpace(category) ? "General" : category;
            Action = action ?? "";
            Message = message ?? "";
            Username = username;
            TraceId = traceId;
            DataJson = dataJson;
            Exception = exception;
            CreatedAtUtc = DateTime.UtcNow;
        }
    }
}
