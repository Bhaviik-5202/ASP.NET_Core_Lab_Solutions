using Lab_18_File_Handling.Services;
using Microsoft.AspNetCore.Http.Features;
using Scalar.AspNetCore;

namespace Lab_18_File_Handling
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var webRoot = builder.Environment.WebRootPath ?? Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
            if (!Directory.Exists(webRoot))
            {
                Directory.CreateDirectory(webRoot);
            }
            builder.Environment.WebRootPath = webRoot;

            var uploadFolderPath = Path.Combine(webRoot, "Files", "Users");
            if (!Directory.Exists(uploadFolderPath))
            {
                Directory.CreateDirectory(uploadFolderPath);
            }

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddScoped<IFileService, FileService>();

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 15 * 1024 * 1024;
            });

            builder.Services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 7253;
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
