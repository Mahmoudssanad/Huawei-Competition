using GreenEye.Application.IRepository;
using GreenEye.Infrasturcture.Data;
using GreenEye.Infrasturcture.Identity;
using GreenEye.Infrasturcture.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GreenEye.Infrasturcture.DependancyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrasturctureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add database connection
            services.AddDbContext<AppDbContext>(options => options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 44))));

            // Add Identity service
            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // create dependancy injection
            services.AddScoped<IAccountRepository, AccountRepository>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this  IApplicationBuilder app)
        {
            return app;
        }
    }
}
