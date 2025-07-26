
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Qlarissa.Application;
using Qlarissa.Application.Interfaces;
using Qlarissa.Domain.Entities;
using Qlarissa.Infrastructure.Persistence;
using Qlarissa.Infrastructure.Persistence.Interfaces;

namespace Qlarissa.WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddIdentity<QlarissaUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IQlarissaUserManager, QlarissaUserManager>();

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger(); // https://localhost:7145/swagger/v1/swagger.json
            app.UseSwaggerUI(); // https://localhost:7145/swagger
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
