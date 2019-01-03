using Notebar.Core;
using Notebar.Core.Icons;
using Notebar.Core.Indicators;
using Notebar.Core.WCF;
using Notebar.WCF;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
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
        private WcfHost Host { get; }

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                IndicatorsService = new IndicatorsService(new IconsService());
                Host = WcfHost.Run(port => IndicatorsService.Add(port));
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("An exception occurred: {0}", ce.Message);
            }

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
            Host.ShutDown();
        }
    }
}
