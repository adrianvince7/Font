using Amazon.CognitoIdentityProvider;
using Font.Core.Interfaces;
using Font.Infrastructure.Authentication;
using Font.Infrastructure.Data;
using Font.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Font.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<FontDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                    b => b.MigrationsAssembly(typeof(FontDbContext).Assembly.FullName)));

            // AWS Services
            services.AddAWSService<IAmazonCognitoIdentityProvider>();

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMeetingRepository, MeetingRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();

            // Authentication
            services.AddScoped<IAuthenticationService, CognitoAuthenticationService>();

            return services;
        }
    }
} 