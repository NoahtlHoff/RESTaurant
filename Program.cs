
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RESTaurang.Data;
using RESTaurang.Services;
using RESTaurang.Services.IServices;

namespace RESTaurang
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddScoped<ITableService, TableService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IMenuItemService, MenuItemService>();

            builder.Services.AddAuthorization(o =>
            {
                o.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                AppDbInitializer.SeedAsync(scope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //app.UseAuthorization();


            app.MapControllers();
            app.Run();
        }
    }
}
