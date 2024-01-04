using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HundredManMario
{
    public partial class MainWindow : Window
    {
        public static MediaElement VideoPlayer;
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
                mvm.OneUpCommand.Execute(null);
            }
            else if (e.Key == Key.Subtract)
            {
                mvm.DeathCommand.Execute(null);
            }
        }

        private void VideoPlayer_OnLoaded(object sender, RoutedEventArgs e)
        {
            VideoPlayer = sender as MediaElement;
        }
    }
}
