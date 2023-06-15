using ProgramTools.ViewModel;

namespace ProgramTools.Pages
{
    /// <summary>
    /// Interaction logic for UninstallPage.xaml
    /// </summary>
    public partial class UninstallPage : INavigableView<UninstallViewModel>
    {
        public UninstallPage(UninstallViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        #region Implementation of INavigableView<out UninstallViewModel>

        /// <inheritdoc />
        public UninstallViewModel ViewModel { get; }

        #endregion
    }
}