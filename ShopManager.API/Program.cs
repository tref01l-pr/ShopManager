namespace ShopManager.API;

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShopManager.Application.Services;
using ShopManager.DataAccess.SqlServer;
using ShopManager.DataAccess.SqlServer.Entities;
using ShopManager.DataAccess.SqlServer.Repositories;
using ShopManager.Domain.Interfaces;
using ShopManager.Domain.Interfaces.Repositories;
using ShopManager.Domain.Options;
using Swashbuckle.AspNetCore.Filters;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        DotNetEnv.Env.Load("../.env");
        builder.Services.Configure<JWTSecretOptions>(options =>
        {
            options.Secret = Environment.GetEnvironmentVariable("JWT_SECRET") ??
                             throw new Exception("JWT_SECRET is not set");
        });

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
            });

            options.OperationFilter<SecurityRequirementsOperationFilter>();
        });

        builder.Services.AddDbContext<ShopManagerDbContext>(options =>
            options.UseSqlServer(
                Environment.GetEnvironmentVariable("DATABASE_URL"),
                x => x.MigrationsAssembly("ShopManager.DataAccess.SqlServer")));

        builder.Services
            .AddIdentity<UserEntity, IdentityRole<Guid>>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<ShopManagerDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        });

        builder.Services.AddAutoMapper(config =>
        {
            config.AddProfile<ApiMappingProfile>();
            config.AddProfile<DataAccessMappingProfile>();
        });

        builder.Services.AddTransient<Seed>();
        
        builder.Services.AddScoped<ICategoriesService, CategoriesService>();
        builder.Services.AddScoped<IOrdersService, OrdersService>();
        builder.Services.AddScoped<IProductsService, ProductsService>();
        builder.Services.AddScoped<IUsersService, UsersService>();

        builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
        builder.Services.AddScoped<IOrderItemsRepository, OrderItemsRepository>();
        builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
        builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
        builder.Services.AddScoped<ISessionsRepository, SessionsRepository>();
        builder.Services.AddScoped<ITransactionsRepository, TransactionsRepository>();
        builder.Services.AddScoped<IUsersRepository, UsersRepository>();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWTSecret:Secret").Value!)),
                };
            });

        builder.Services.AddAuthorization();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Any", corsPolicyBuilder =>
            {
                corsPolicyBuilder
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        var app = builder.Build();

        if (args.Length == 1 && args[0].ToLower() == "--seeddata")
            await SeedData(app);

        async Task SeedData(IHost app)
        {
            var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

            using (var scope = scopedFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<Seed>();
                await service.SeedDataContextAsync();
            }
        }


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors("Any");

        app.MapControllers();

        app.Run();
    }
}