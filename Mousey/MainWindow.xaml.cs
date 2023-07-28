using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Mousey;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MouseyService _mouseService;
        private Settings _settings;

        public MainWindow()
        {
            _mouseService = new MouseyService();
            InitializeComponent();
            _settings = Settings.Load();
            txtInterval.Text = _settings.Interval.ToString();
        }

        private TimeSpan ParseTime()
        {
            try
            {
                return TimeSpan.Parse(txtInterval.Text);
            }
            catch (Exception e)
            {
                return TimeSpan.Zero;
            }
        }

        private void Toggle_OnClick(object sender, RoutedEventArgs e)
        {
            if (_mouseService.IsRunning)
            {
                _mouseService.Stop();
                btnToggle.Content = "Start";
                return;
            }

            // var time = ParseTime();
            // if (time == TimeSpan.Zero)
            // {
            //     txtInterval.Text = TimeSpan.Zero.ToString();
            //     return;
            // }

            _mouseService.Start(_settings.Interval);
            btnToggle.Content = "Stop";
        }

        private void onSave_Click(object sender, RoutedEventArgs e)
        {
            _settings.SaveToFile();
        }


        private void TxtInterval_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var newTime = ParseTime();
            if (newTime == TimeSpan.Zero)
            {
                txtInterval.Text = _settings.Interval.ToString();
                return;
            }

            _settings.Interval = newTime;
        }
    }
}