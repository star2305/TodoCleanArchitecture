using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoCleanArchitecture.Application.Interfaces
{
    public interface IAuditLogWriter
    {
        Task WriteAsync(
            string level,
            string category,
            string action,
            string message,
            string? username = null,
            string? traceId = null,
            string? dataJson = null,
            string? exception = null);
    }
}
