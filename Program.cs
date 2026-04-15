using AnimalClinic.Grpc;
using AnimalClinic.Middlewares;
using AnimalClinicLogic;
using AnimalClinicLogic.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

            builder.Services.AddGrpc(options =>
            {
                options.Interceptors.Add<AnimalClinic.Grpc.GrpcLoggingInterceptor>();
            });


            var key = "f8Dk2!sL9@qPzX7#vB4mN6cR1tYwE3uH";

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(key)
                        )
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Cookies.ContainsKey("jwt"))
                            {
                                context.Token = context.Request.Cookies["jwt"];
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddSingleton<DB>();
            builder.Services.AddScoped<AdminService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<UserService>();

            builder.Services.AddSingleton<IConnectionMultiplexer>(
                    ConnectionMultiplexer.Connect("localhost:6379")
                );



            var app = builder.Build();

            //app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<LoggerMiddleware>();


            app.MapGrpcService<ClinicStatsGrpcService>();
            app.MapControllers();
            app.MapGet("/", async (context) => context.Response.Redirect("/swagger"));

            app.Run();
        }
    }
}
