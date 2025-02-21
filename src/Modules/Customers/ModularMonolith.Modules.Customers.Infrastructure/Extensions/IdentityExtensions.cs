using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Modules.Customers.Application.Interfaces;
using ModularMonolith.Modules.Customers.Domain.Entities;
using ModularMonolith.Modules.Customers.Infrastructure.Identity.Services;
using ModularMonolith.Modules.Customers.Infrastructure.Persistence;
using ModularMonolith.Modules.Customers.Infrastructure.Services;
using ModularMonolith.Shared.Infrastructure.Auth;

namespace ModularMonolith.Modules.Customers.Infrastructure.Extensions;

public static class IdentityExtensions
{
    public static IServiceCollection AddCustomerModuleIdentity(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddIdentity<Customer, IdentityRole<Guid>>(options =>
            {
                // Password settings
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;

                // User settings
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<CustomerModuleDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<JwtProvider>();
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}