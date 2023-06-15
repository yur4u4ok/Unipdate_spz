using CommunityToolkit.Mvvm.ComponentModel;

using System.Windows;

namespace ProgramTools.ViewModel
{
    public class HelpViewModel : ObservableObject
    {
        public string HelpText { get; }
    
        public HelpViewModel()
        {
            var dict = Application.Current.Resources.MergedDictionaries[2];
            var text = (string)dict["HelpWelcomeText"];
            HelpText = text;
        }
    }
}