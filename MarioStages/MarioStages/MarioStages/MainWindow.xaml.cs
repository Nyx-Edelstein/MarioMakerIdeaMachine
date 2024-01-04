using System.Windows;
using System.Windows.Input;

namespace MarioStages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            var mvm = DataContext as MainViewModel;
            if (mvm == null) return;

            if (e.Key == Key.Add)
            {
                mvm.CurrentStage += 1;
            }

            if (e.Key == Key.Subtract)
            {
                mvm.CurrentStage = 1;
            }
        }
    }
}
