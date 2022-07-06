using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Diagnostics;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ThunderDistance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ContentRendered += (s, e) => { timerInitialize(); };
            InitializeComponent();
        }
        private bool _isStart = false; // trueでStart状態
        private double _maha = 340;
        private Stopwatch _sw = new System.Diagnostics.Stopwatch();
        private DispatcherTimer _timer;
        private void timerInitialize()
        {
            _timer = new DispatcherTimer(DispatcherPriority.Background);
            _timer.Interval = TimeSpan.FromMilliseconds(8);
            _timer.Tick += new EventHandler(tick);
        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _isStart = !_isStart;
            distLabel.Foreground = new SolidColorBrush(Color.FromArgb(0xDD, 0x00, 0x00, 0x00));
            btnRenew();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            _isStart = false;
            reset();
        }
        private void btnRenew()
        {
            if (_isStart)
            {
                startButton.Text = "Stop";
                start();
            }
            else
            {
                startButton.Text = "Start";
                stop();
            }
        }
        private void start()
        {
            _sw.Start();
            _timer.Start();
        }
        private void stop()
        {
            _sw.Stop();
            _timer.Stop();

            TimeSpan ts = _sw.Elapsed;
            int s = ts.Seconds;

            double sDouble = s + 0.001 * ts.Milliseconds;
            double dist = sDouble * _maha;
            int dot = (int)((dist - (int)dist) * 10) % 10;
            string str;
            if (s == 0 && dot == 0) str = string.Format($"約{(int)dist:0000}.{dot:d1}m");
            else
            {
                str = string.Format($"雷は約{(int)dist:0000}.{dot:d1}m\n離れた地点に落ちました");
                distLabel.Foreground = new SolidColorBrush(Color.FromArgb(0xDD, 0xFF, 0x00, 0x00));
            }
            distLabel.Text = str;
        }

        private void reset()
        {
            stop();
            _sw.Reset();
            tick(null, null);
            btnRenew();
            distLabel.Foreground = new SolidColorBrush(Color.FromArgb(0xDD, 0x00, 0x00, 0x00));
        }


        private void tick(object sender, EventArgs e)
        {
            TimeSpan ts = _sw.Elapsed;
            int s = ts.Seconds;
            int dot = ts.Milliseconds / 10; // 小数第2位
            string str = $"{s:d2}.{dot:d2}s 経過";
            timeLabel.Text = str;

            double sDouble = s + 0.001 * ts.Milliseconds;
            double dist = sDouble * _maha;
            dot = (int)((dist - (int)dist) * 10) % 10;
            str = string.Format($"約{(int)dist:0000}.{dot:d1}m");
            distLabel.Text = str;
        }
        private void TextChanged(object sender, EventArgs e)
        {
            if (!double.TryParse(mahaTBox.Text, out _maha)) _maha = 340;
            mahaTBox.Text = $"{_maha:F1}";
        }
    }
}
