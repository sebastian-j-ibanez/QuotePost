using Microsoft.EntityFrameworkCore;
using ProblemAnalysis3.DataAccess;

namespace ProblemAnalysis3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connStr = builder.Configuration.GetConnectionString("PA3Db");
            builder.Services.AddDbContext<QuoteDbContext>(options => options.UseSqlServer(connStr));

            builder.Services.AddControllers(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
            });

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.MapControllers();

            app.Run();
        }
    }
}
