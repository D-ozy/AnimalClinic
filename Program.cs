using AnimalClinic.Services;
using Microsoft.Extensions.Primitives;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnimalClinic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<DB>();
            builder.Services.AddScoped<AnimalService>();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseMiddleware<LoggerMiddleware>();

            app.MapGet("/", async (context) => context.Response.Redirect("/swagger"));
            app.MapControllers();

            app.Run();
        }
    }
}
