using Notifications.Wpf.Core;

using System.Threading.Tasks;

namespace ProgramTools.Service
{
    public interface INotificationService
    {
        Task ShowAsync(string title, string content, NotificationType type);
    }

    public class NotificationService : INotificationService
    {
        private readonly INotificationManager _manager;
        public NotificationService()
        {
            _manager = new NotificationManager();
        }

        private static NotificationContent CreateContent(string windowTitle = "", string text = "", NotificationType type =NotificationType.Information)
        {
            return new NotificationContent()
            {
                Message = text,         //Текст повідомлення
                Title = windowTitle,    //Заголовок повідомлення
                Type = type             //Тип повідомлення
            };
        }


        public Task ShowAsync(string title, string content, NotificationType type) => _manager.ShowAsync(CreateContent(title, content, type));
    }
}