using ProgramTools.ViewModel;

namespace ProgramTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INavigableView<MainWindowViewModel>
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        #region Implementation of INavigableView<out MainWindowViewModel>

        /// <inheritdoc />
        public MainWindowViewModel ViewModel { get; }

        #endregion
    }
}