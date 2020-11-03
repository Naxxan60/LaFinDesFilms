using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace ConsoleFinDesFilms
{
    class Program
    {
        static void Main()
        {
            // Create service collection and configure our services
            var services = ConfigureServices();
            // Generate a provider
            var serviceProvider = services.BuildServiceProvider();

            // Kick off our actual code
            serviceProvider.GetService<ConsoleApplication>().Run();
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            IConfiguration config = LoadConfiguration();
            services.AddSingleton(config);
            services.AddTransient<IUpdateAllMoviesProcess, UpdateAllMoviesProcess>();
            services.AddTransient<IUpdateTopRatedMoviesProcess, UpdateTopRatedMoviesProcess>();
            services.AddDbContext<FilmContext>(opts => opts.UseSqlServer(config.GetConnectionString("FinDesFilmsDatabase")));
            services.AddTransient<ConsoleApplication>();
            return services;
        }
        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}
