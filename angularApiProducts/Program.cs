using angularApiProducts.Data;
using angularApiProducts.Models;
using Microsoft.EntityFrameworkCore;

namespace angularApiProducts
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Tilf�j CORS-tjeneste til DI-containeren
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200")  // Din frontend URL
                            .AllowAnyMethod()  // Tillad alle HTTP-metoder (GET, POST, PUT, DELETE)
                            .AllowAnyHeader()  // Tillad alle headers
                            .AllowCredentials();  // Tillad cookies (valgfrit)
                    });
            });

            // Tilf�j de andre services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Tilf�j databasen (EF Core)
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Aktiver Swagger UI, kun i udviklingsmilj�
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Aktiver CORS-politik
            app.UseCors("AllowLocalhost");

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Map controllers
            app.MapControllers();

            app.Run();
        }
    }
}
