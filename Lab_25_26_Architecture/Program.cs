using Lab_25_26_Architecture.Repositories;
using Lab_25_26_Architecture.Services;
using Lab_25_26_Architecture.Validators;
using Scalar.AspNetCore;

namespace Lab_25_26_Architecture
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 7021;
            });

            builder.Services.AddOpenApi();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<UserValidator>();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
