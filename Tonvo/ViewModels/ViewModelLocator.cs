using MySqlConnector;

namespace Tonvo.ViewModels
{
    internal class ViewModelLocator
    {
        private static ServiceProvider? _provider;
        private static ServiceCollection _services;

        public static void Init(ServiceProvider provider, ServiceCollection services)
        {
            _provider = provider;

            _services = services;

            services.AddTransient<ApplicantControlPanelViewModel>();
            services.AddTransient<ApplicantFieldsViewModel>();
            services.AddTransient<CompanyControlPanelViewModel>();
            services.AddTransient<CompanyFieldsViewModel>();
            services.AddTransient<BrowseListViewModel>();
            services.AddTransient<ApplicantAccountViewModel>();
            services.AddTransient<RootViewModel>();
            services.AddTransient<ShellViewModel>();
            services.AddTransient<SignInViewModel>();
            services.AddTransient<SignUpViewModel>();
            services.AddTransient<VacancyFieldsViewModel>();

            _provider = services.BuildServiceProvider();
            foreach (var service in services)
            {
                _provider.GetRequiredService(service.ServiceType);
            }
        }

        public static ApplicantControlPanelViewModel? ApplicantControlPanelViewModel => _provider?.GetRequiredService<ApplicantControlPanelViewModel>();
        public static ApplicantFieldsViewModel? ApplicantFieldsViewModel => _provider?.GetRequiredService<ApplicantFieldsViewModel>();
        public static CompanyControlPanelViewModel? CompanyControlPanelViewModel => _provider?.GetRequiredService<CompanyControlPanelViewModel>();
        public static CompanyFieldsViewModel? CompanyFieldsViewModel => _provider?.GetRequiredService<CompanyFieldsViewModel>();
        public static BrowseListViewModel? BrowseListViewModel => _provider?.GetRequiredService<BrowseListViewModel>();
        public static ApplicantAccountViewModel? ApplicantAccountViewModel => _provider?.GetRequiredService<ApplicantAccountViewModel>();
        public static RootViewModel? RootViewModel => _provider?.GetRequiredService<RootViewModel>();
        public static ShellViewModel? ShellViewModel => _provider?.GetRequiredService<ShellViewModel>();
        public static SignInViewModel? SignInViewModel => _provider?.GetRequiredService<SignInViewModel>();
        public static SignUpViewModel? SignUpViewModel => _provider?.GetRequiredService<SignUpViewModel>();
        public static VacancyFieldsViewModel? VacancyFieldsViewModel => _provider?.GetRequiredService<VacancyFieldsViewModel>();
    }
}
