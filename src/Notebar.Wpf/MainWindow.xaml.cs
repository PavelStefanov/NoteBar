using Notebar.Core.Grpc;
using Notebar.Core.Icons;
using Notebar.Core.Indicators;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Notebar.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IndicatorsService IndicatorsService { get; }
        private GrpcServer GrpcServer { get; }

        public MainWindow()
        {
            InitializeComponent();

            IndicatorsService = new IndicatorsService(new IconsService());
            GrpcServer = GrpcServer.Run(port =>
             {
                 return Dispatcher.Invoke(() =>
                  {
                      return IndicatorsService.Add(port);
                  });
             });

            FieldsListBox.ItemsSource = IndicatorsService.Indicators;
        }

        private void OnIndicatorQuit(object sender, RoutedEventArgs e)
        {
            var indicator = (Indicator)((MenuItem)sender).DataContext;
            IndicatorsService.Remove(indicator);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            IndicatorsService.ShutDownAll();
            GrpcServer.ShutDown();
        }
    }
}
