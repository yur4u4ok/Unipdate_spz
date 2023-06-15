using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ProgramTools.Models;
using ProgramTools.Service;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ProgramTools.ViewModel
{
    public partial class UninstallViewModel : ObservableObject
    {
        [ObservableProperty] private ObservableCollection<InstalledApplication> _apps = new();
        [ObservableProperty] private InstalledApplication _selectedApplication;
        [ObservableProperty] private string _searchText;
        private readonly IUninstallService _uninstallService;
        private readonly INotificationService _notificationService;
        private List<InstalledApplication> _cachedApps;

        public UninstallViewModel(IUninstallService uninstallService, INotificationService notificationService)
        {
            _uninstallService = uninstallService;
            _notificationService = notificationService;
            SearchText = string.Empty;
            Dispatcher.CurrentDispatcher.Invoke(async () => await _notificationService.ShowAsync("Зачекайте", "Йде пошук програм, це може зайняти деякий час", Notifications.Wpf.Core.NotificationType.Information));
            Task.Run(async () => 
            {
                var list = await _uninstallService.GetAllInstalledApps();
                _cachedApps = list.ToList();
                Apps = new ObservableCollection<InstalledApplication>(_cachedApps);
            });
        }

        [RelayCommand]
        private async Task Search()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await _notificationService.ShowAsync("Зачекайте", "Пошук розпочато, це може зайняти деякий час", Notifications.Wpf.Core.NotificationType.Information);
                Apps = new ObservableCollection<InstalledApplication>(_cachedApps);
            }
            else
            {
                Apps = new ObservableCollection<InstalledApplication>(_cachedApps.Where(app => app.Name.ToLower().Contains(SearchText.ToLower())));
            }
        }

        [RelayCommand]
        private async Task CheckUpdates()
        {
            await _notificationService.ShowAsync("Зачекайте", "Перевірка оновлень розпочата", Notifications.Wpf.Core.NotificationType.Information);
            var installedUpdatesList = await _uninstallService.GetInstalledApplicationsToUpdate();
            if (installedUpdatesList != null)
            {
                var window = new UpdateWindow(installedUpdatesList);
                window.ShowDialog();
            }
        }
        [RelayCommand]
        private async Task Uninstall(InstalledApplication application)
        {
            if (application is null)
            {
                await _notificationService.ShowAsync("Помилка", "Оберіть програму для видалення", Notifications.Wpf.Core.NotificationType.Error);
                return;
            }

            var confirmation = MessageBox.Show("Точно?", "Попередження", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (confirmation != MessageBoxResult.Yes)
            {
                return;
            }

            await _notificationService.ShowAsync("Зачекайте", "Видалення може зайняти певний час", Notifications.Wpf.Core.NotificationType.Information);
            var uninstallResult = _uninstallService.Uninstall(application.ProductCode);
            if (!uninstallResult)
            {
                await _notificationService.ShowAsync("Помилка", "Видалення не вдалося", Notifications.Wpf.Core.NotificationType.Error);
                return;
            }
        }
    }
}
