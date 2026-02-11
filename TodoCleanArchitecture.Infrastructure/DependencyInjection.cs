using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoCleanArchitecture.Application.Interfaces;
using TodoCleanArchitecture.Infrastructure.Persistence;
using TodoCleanArchitecture.Infrastructure.Repositories;
using TodoCleanArchitecture.Infrastructure.Security;

namespace TodoCleanArchitecture.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TodoDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ITodoRepository, TodoRepository>();
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<IUserAuthService, DemoUserAuthService>();

            return services;
        }
    }
}
