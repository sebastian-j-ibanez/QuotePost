using Microsoft.EntityFrameworkCore;
using ProblemAnalysis3.DataAccess;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connStr = builder.Configuration.GetConnectionString("PA3Db");
            builder.Services.AddDbContext<QuoteDbContext>(options => options.UseSqlServer(connStr));

            builder.Services.AddControllers();

            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
