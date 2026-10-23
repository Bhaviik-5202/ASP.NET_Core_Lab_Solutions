using FluentValidation;
using Lab_20_21_Validation.Validators;
using Scalar.AspNetCore;

namespace Lab_20_21_Validation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 7237;
            });

            builder.Services.AddOpenApi();

            builder.Services.AddValidatorsFromAssemblyContaining<StudentBasicValidator>();

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
