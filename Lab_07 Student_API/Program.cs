using Scalar.AspNetCore;

namespace Lab_07_Student_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 7168;
            });

            builder.Services.AddOpenApi();

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
