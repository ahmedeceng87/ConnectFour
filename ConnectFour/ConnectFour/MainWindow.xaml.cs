using System.Windows;
using System.Windows.Input;
using ConnectFour.ViewModels;

namespace ConnectFour
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void Cell_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is CellViewModel cell)
            {
                // We only care about the column of the clicked cell
                int col = cell.Col;

                if (DataContext is MainWindowViewModel vm && vm.DropCommand.CanExecute(col))
                {
                    vm.DropCommand.Execute(col);
                }
            }
        }
    }
}