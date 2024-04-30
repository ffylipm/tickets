
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
            builder.Services.AddDbContext<TicketsContext>(o => o.UseSqlServer(Configuration.GetConnectionString("Tickets")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
