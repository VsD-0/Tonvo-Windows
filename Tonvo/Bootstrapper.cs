namespace Tonvo
{
    public class Bootstrapper
    {
        private static ServiceProvider? _provider;
        public static IConfiguration? Configuration { get; private set; }

        public static void Init()
        {
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
            var services = new ServiceCollection();

            services.AddDbContext<DbTonvoContext>(options =>
            {
                var conn = Configuration.GetConnectionString("DefaultConnection");
                options.UseMySql(conn, ServerVersion.AutoDetect(conn));
            }, ServiceLifetime.Transient);

            _provider = services.BuildServiceProvider();
        }

        private static void RegisterServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton(Configuration);
            services.AddSingleton<ApplicantService>();
            services.AddSingleton<CompanyService>();
            services.AddSingleton<VacancyService>();
            services.AddSingleton<FavoriteService>();
            services.AddSingleton<INavigationService, NavigationService>();

            _provider = services.BuildServiceProvider();
        }
    }
}