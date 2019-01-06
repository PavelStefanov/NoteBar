using CSDeskBand;
using Notebar.Core;
using Notebar.Core.Icons;
using Notebar.Core.Indicators;
using Notebar.Core.WCF;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace Notebar.Toolbar
{
    [ComVisible(true)]
    [Guid(Constants.NotebarGuid)]
    [CSDeskBandRegistration(Name = "Notebar", ShowDeskBand = true)]
    public partial class DeskBand : INotifyPropertyChanged
    {
        private IndicatorsService IndicatorsService { get; }
        private WcfHost Host { get; }

        private Orientation _orientation;

        public Orientation Orientation
        {
            get => _orientation;
            set
            {
                if (value == _orientation)
                    return;

                _orientation = value;
                OnPropertyChanged();
            }
        }

        public DeskBand()
        {
            InitializeComponent();
            Options.IsFixed = true;

            try
            {
                IndicatorsService = new IndicatorsService(new IconsService(), RemoveIndicator);
                Host = WcfHost.Run(port =>
                {
                    var result = IndicatorsService.Add(port);
                    if (!string.IsNullOrEmpty(result))
                        return result;

                    UpdateSize();
                    return null;
                });
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("Notebar", e.Message, EventLogEntryType.Error);
                throw;
            }

            Indicators.ItemsSource = IndicatorsService.Indicators;

            UpdateOrientation(TaskbarInfo.Orientation);
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
                try
                {
                    IndicatorsService.Remove(indicator);
                    UpdateSize();
                }
                catch (Exception exception)
                {
                    EventLog.WriteEntry("Notebar", exception.Message, EventLogEntryType.Error);
                    throw;
                }
            });
        }

        protected override void OnClose()
        {
            base.OnClose();
            IndicatorsService.ShutDownAll();
            Host.ShutDown();
        }

        private void UpdateOrientation(TaskbarOrientation taskbarOrientation)
        {
            Orientation = OrientationHelper.Map(taskbarOrientation);
        }

        private void UpdateSize()
        {
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    Options.HorizontalSize.Width = Options.MinHorizontalSize.Width = IndicatorsService.Indicators.Count * 30;
                    break;
                case Orientation.Vertical:
                    Options.VerticalSize.Height = Options.MinVerticalSize.Height = IndicatorsService.Indicators.Count * 29;
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
