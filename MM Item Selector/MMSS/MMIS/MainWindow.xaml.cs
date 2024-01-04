using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MMIM
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LikaLink.RequestNavigate += (sender, e) =>
            {
                System.Diagnostics.Process.Start(e.Uri.ToString());
            };

            Task.Run(() =>
            {
                while (_running)
                {
                    Thread.Sleep(100);
                    if (_mouseCaptured || InfoText.IsMouseOver || !Info.IsVisible) continue;

                    var timeSinceCapture = (DateTime.Now - _lastMouseCapture).TotalSeconds;
                    if (timeSinceCapture < 1) continue;

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Info.Visibility = Visibility.Hidden;
                    });
                }
            });
        }

        ~MainWindow()
        {
            _running = false;
        }

        private static bool _running = true;
        private static bool _mouseCaptured;
        private static DateTime _lastMouseCapture = DateTime.Now;

        private void InfoImage_OnMouseEnter(object sender, MouseEventArgs e)
        {
            _mouseCaptured = true;
            _lastMouseCapture = DateTime.Now;
            Info.Visibility = Visibility.Visible;
        }

        private void InfoImage_OnMouseLeave(object sender, MouseEventArgs e)
        {
            _mouseCaptured = false;
        }

        private void InfoImage_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.twitch.tv/likandoo");
        }

        private void Slider_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var slider = sender as Slider;
            if (slider == null) return;

            var delta = e.Delta < 0 ? -1 : 1;

            slider.Value = slider.Value + delta < slider.Minimum
                ? slider.Minimum
                : slider.Value + delta > slider.Maximum
                ? slider.Maximum
                : slider.Value + delta;
        }
    }
}
