using MySqlConnector;
using System.Diagnostics;

namespace Tonvo
{
    public class Bootstrapper
    {
        public static ServiceProvider? Provider { get; private set; }
        public static ServiceCollection? Services { get; private set; }
        public static IConfiguration? Configuration { get; private set; }

        public static void Init()
        {
            Services = new ServiceCollection();
            ConfigureConfiguration();
            ConfigureDatabase();
            RegisterServices();
        }

        private static void ConfigureConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
        }

        private static void ConfigureDatabase()
        {
            Services.AddDbContext<DbTonvoContext>(options =>
            {
                var conn = Configuration.GetConnectionString("DefaultConnection");
                try { options.UseMySql(conn, ServerVersion.AutoDetect(conn)); }
                catch (MySqlException ex) { Debug.WriteLine($"\n------------------------------------------------------------------------------------------\n" +
                                                            $"Ошибка подключения к серверу MySQL: {ex.Message}" +
                                                            $"\n------------------------------------------------------------------------------------------\n"); }
            }, ServiceLifetime.Transient);

            Provider = Services.BuildServiceProvider();
        }

        private static void RegisterServices()
        {
            Services.AddSingleton(Configuration);
            Services.AddSingleton<ApplicantService>();
            Services.AddSingleton<CompanyService>();
            Services.AddSingleton<VacancyService>();
            Services.AddSingleton<FavoriteService>();
            Services.AddSingleton<INavigationService, NavigationService>();
            Services.AddSingleton<INavigationServiceForBrowse, NavigationService>();
            Services.AddSingleton<INavigationServiceForControl, NavigationService>();
            Services.AddSingleton<IMessageBus, MessageBus>();

            Provider = Services.BuildServiceProvider();
        }
    }
}