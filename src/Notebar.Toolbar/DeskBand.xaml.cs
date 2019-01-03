using CSDeskBand;
using Notebar.Core.Icons;
using Notebar.Core.Indicators;
using Notebar.Core.WCF;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Size = CSDeskBand.Size;

namespace Notebar.Toolbar
{
    [ComVisible(true)]
    [Guid("32da9796-4495-4157-9aac-d1d7564c4119")]
    [CSDeskBandRegistration(Name = "Notebar", ShowDeskBand = true)]
    public partial class DeskBand
    {
        private IndicatorsService IndicatorsService { get; }
        private WcfHost Host { get; }

        public DeskBand()
        {
            InitializeComponent();
            Options.HorizontalSize = new Size(20, 20);
            Options.VerticalSize = new Size(20, 20);

            try
            {
                IndicatorsService = new IndicatorsService(new IconsService(), Dispatcher);
                Host = WcfHost.Run(port => IndicatorsService.Add(port));
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Notebar", e.Message, EventLogEntryType.Error);
                throw;
            }

            FieldsListBox.ItemsSource = IndicatorsService.Indicators;
        }

        private void OnIndicatorQuit(object sender, RoutedEventArgs e)
        {
            var indicator = (Indicator)((MenuItem)sender).DataContext;

            try
            {
                IndicatorsService.Remove(indicator);
            }
            catch (Exception exception)
            {
                EventLog.WriteEntry("Notebar", exception.Message, EventLogEntryType.Error);
                throw;
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
            IndicatorsService.ShutDownAll();
            Host.ShutDown();
        }
    }
}
