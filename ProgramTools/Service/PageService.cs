using System;
using System.Windows;

namespace ProgramTools.Service
{
    public interface IPageService
    {
        FrameworkElement GetPage(Type viewType);
    }

    public class PageService : IPageService
    {
        private readonly IServiceProvider _serviceProvider;

        public PageService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #region Implementation of IPageService

        /// <inheritdoc />
        public FrameworkElement GetPage(Type viewType)
        {
            return (FrameworkElement)_serviceProvider.GetService(viewType);
        }

        #endregion
    }
}