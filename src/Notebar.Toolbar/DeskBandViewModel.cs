using CSDeskBand;
using Notebar.Core.Icons;
using Notebar.Core.Indicators;
using Notebar.Core.WCF;
using Notebar.Toolbar.Helpers;
using Notebar.Toolbar.MVVM;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Notebar.Toolbar
{
    public class DeskBandViewModel : INotifyPropertyChanged
    {
        private const int IndicatorItemHeight = 26;
        private const int IndicatorItemWidth = 26;

        private CSDeskBandOptions Options { get; }
        private Dispatcher Dispatcher { get; }
        private IndicatorsService IndicatorsService { get; set; }
        private WcfHost Host { get; set; }

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

        public ObservableCollection<Indicator> Indicators => IndicatorsService?.Indicators;

        public DeskBandViewModel(CSDeskBandOptions options, Dispatcher dispatcher)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            Dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        public void Init(TaskbarOrientation taskbarOrientation)
        {
            Options.IsFixed = true;
            Orientation = OrientationHelper.Map(taskbarOrientation);

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

            UpdateSize();
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

        private RelayCommand _indicatorQuit;
        public RelayCommand IndicatorQuit => _indicatorQuit ??
            (_indicatorQuit = new RelayCommand(param =>
            {
                if (!(param is Indicator indicator))
                    return;

                RemoveIndicator(indicator);
            }));

        private void UpdateSize()
        {
            var dpiOptions = DpiHelper.GetDpiOptions();

            double size = 0;
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    size = IndicatorsService.Indicators.Count * IndicatorItemWidth * dpiOptions.WidthFactor;
                    Options.HorizontalSize.Width = Options.MinHorizontalSize.Width = Convert.ToInt32(size);
                    break;
                case Orientation.Vertical:
                    size = IndicatorsService.Indicators.Count * IndicatorItemHeight * dpiOptions.HeightFactor;
                    Options.VerticalSize.Height = Options.MinVerticalSize.Height = Convert.ToInt32(size);
                    break;
            }
        }

        public void OnClose()
        {
            IndicatorsService.ShutDownAll();
            Host.ShutDown();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
