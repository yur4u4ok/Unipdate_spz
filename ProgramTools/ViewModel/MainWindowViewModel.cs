using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ProgramTools.Pages;
using ProgramTools.Service;

using System;
using System.Threading.Tasks;
using System.Windows;

namespace ProgramTools.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly IPageService _pageService;
        [ObservableProperty]
        private FrameworkElement _view;

        public MainWindowViewModel(IPageService pageService)
        {
            _pageService = pageService;
            View = _pageService.GetPage(typeof(HelpPage));
        }

        [RelayCommand]
        private void ChangeView(Type viewType)
        {
            if (viewType is null)
            {
                return;
            }

            if (View.GetType().IsAssignableFrom(viewType))
            {
                return;
            }

            var view = _pageService.GetPage(viewType);
            View = view;
        }

        [RelayCommand]
        private void ChangeLanguage()
        {
            var currentLanguage = Application
                .Current
                .Resources
                .MergedDictionaries[2]
                .Source
                .ToString()
                .Split(".")[^2];
            ResourceDictionary resourceDictionary;
            if (currentLanguage == "en")
            {
                resourceDictionary = new ResourceDictionary
                {
                    Source = new Uri("Localisation/Resources.ua.xaml", UriKind.Relative)
                };
                Application.Current.Resources.MergedDictionaries[2] = resourceDictionary;
                return;
            }

            resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("Localisation/Resources.en.xaml", UriKind.Relative)
            };
            Application.Current.Resources.MergedDictionaries[2] = resourceDictionary;
        }
    }
}