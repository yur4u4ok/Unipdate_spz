using ProgramTools.ViewModel;

namespace ProgramTools.Pages
{
    /// <summary>
    /// Interaction logic for HelpPage.xaml
    /// </summary>
    public partial class HelpPage : INavigableView<HelpViewModel>
    {
        public HelpPage(HelpViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        #region Implementation of INavigableView<out HelpViewModel>

        /// <inheritdoc />
        public HelpViewModel ViewModel { get; }

        #endregion
    }
}