
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Tickets.API.Service;
using Tickets.Persistence;

namespace Tickets.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var Configuration = builder.Configuration;


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<MenuService>();
            builder.Services.AddScoped<RolService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<EventService>();
            builder.Services.AddScoped<PlaceService>();
            builder.Services.AddScoped<ServicesSvc>();
            builder.Services.AddScoped<TicketsService>();
            builder.Services.AddDbContext<TicketsContext>(o => o.UseSqlServer(Configuration.GetConnectionString("Tickets")));
            builder.Services.AddCors();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                                                    //.WithOrigins("https://localhost:44351")); // Allow only this origin can also have multiple origins separated with comma
                .AllowCredentials()); // allow credentials

            app.MapControllers();

            app.Run();
        }
    }
}
