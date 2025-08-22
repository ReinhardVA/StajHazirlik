
using CQRS_example_project.Application.Handlers;
using CQRS_example_project.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CQRS_example_project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("ToDosDB"));

            builder.Services.AddScoped<CreateToDoHandler>();
            builder.Services.AddScoped<GetToDosHandler>();

            var app = builder.Build();
            
            app.Run();
        }
    }
}
