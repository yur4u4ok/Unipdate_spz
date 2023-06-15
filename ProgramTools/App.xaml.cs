using Microsoft.Extensions.DependencyInjection;

using System;
using System.Windows;

namespace ProgramTools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var collection = new ServiceCollection();
            collection.Inject();
            _serviceProvider = collection.BuildServiceProvider();
            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            MainWindow.Show();
        }

    


        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            ((IDisposable)_serviceProvider).Dispose();
        }
    }
}