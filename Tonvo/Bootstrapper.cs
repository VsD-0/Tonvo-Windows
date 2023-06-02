namespace Tonvo
{
    public class Bootstrapper
    {
        private static ServiceProvider? _provider;
        public static IConfiguration? Configuration { get; private set; }
        public static void Init()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            var services = new ServiceCollection();

            services.AddSingleton(Configuration);

            #region Connection

            services.AddDbContext<DbTonvoContext>(options =>
            {
                var conn = Configuration.GetConnectionString("DefaultConnection");
                options.UseMySql(conn, ServerVersion.AutoDetect(conn));
            }, ServiceLifetime.Transient);

            #endregion

            #region Services
            services.AddSingleton<ApplicantService>();
            services.AddSingleton<CompanyService>();
            services.AddSingleton<VacancyService>();
            services.AddSingleton<FavoriteService>();
            services.AddSingleton<NavigationService>();
            #endregion

            _provider = services.BuildServiceProvider();
            foreach (var service in services)
            {
                _provider.GetRequiredService(service.ServiceType);
            }
        }
    }
}
