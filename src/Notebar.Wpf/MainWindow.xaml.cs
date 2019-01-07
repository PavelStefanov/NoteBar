using Notebar.Core.Icons;
using Notebar.Core.Indicators;
using Notebar.Core.WCF;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace Notebar.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IndicatorsService IndicatorsService { get; }
        private WcfHost Host { get; }

        public MainWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow.Height = 40;

            IndicatorsService = new IndicatorsService(new IconsService(), RemoveIndicator);
            Host = WcfHost.Run(port =>
            {
                var result = IndicatorsService.Add(port);
                if (!string.IsNullOrEmpty(result))
                    return result;

                UpdateSize();
                return null;
            });

            Indicators.ItemsSource = IndicatorsService.Indicators;
            UpdateSize();
        }

        private void OnIndicatorQuit(object sender, RoutedEventArgs e)
        {
            var indicator = (Indicator)((MenuItem)sender).DataContext;
            RemoveIndicator(indicator);
        }

        private void RemoveIndicator(Indicator indicator)
        {
            Dispatcher.Invoke(() =>
            {
                IndicatorsService.Remove(indicator);
                UpdateSize();
            });
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            IndicatorsService.ShutDownAll();
            Host.ShutDown();
        }

        private void UpdateSize()
        {
            Matrix transformToDevice;
            using (var source = new HwndSource(new HwndSourceParameters()))
                transformToDevice = source.CompositionTarget.TransformToDevice;

            double DpiWidthFactor = transformToDevice.M11;
            double DpiHeightFactor = transformToDevice.M22;

            Indicators.Width = Indicators.MinWidth = Convert.ToInt32(IndicatorsService.Indicators.Count * 26 * DpiWidthFactor);
        }
    }
}
