using Microsoft.Extensions.DependencyInjection;

using ProgramTools.Pages;
using ProgramTools.Service;
using ProgramTools.ViewModel;

namespace ProgramTools
{
    public static class DepenencyInjection
    {
        public static void Inject(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<INotificationService, NotificationService>();
            serviceCollection.AddSingleton<IUninstallService, UninstallService>();
            serviceCollection.AddSingleton<IPageService, PageService>();
            serviceCollection.AddSingleton<MainWindowViewModel>();
            serviceCollection.AddTransient<HelpPage>();
            serviceCollection.AddTransient<HelpViewModel>();
            serviceCollection.AddTransient<UninstallPage>();
            serviceCollection.AddTransient<UninstallViewModel>();
            serviceCollection.AddSingleton<MainWindow>();
        }
    }
}