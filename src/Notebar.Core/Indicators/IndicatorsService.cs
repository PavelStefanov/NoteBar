using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Notebar.Core.Icons;

namespace Notebar.Core.Indicators
{
    public class IndicatorsService
    {
        public readonly ObservableCollection<Indicator> Indicators = new ObservableCollection<Indicator>();

        public IconsService IconsService { get; }

        public IndicatorsService(IconsService iconsService)
        {
            IconsService = iconsService ?? throw new ArgumentNullException(nameof(iconsService));
        }

        public string Add(uint port)
        {
            if (Indicators.Any(i => i.Port == port))
            {
                return "Already started";
            }

            Indicator indicator;
            try
            {
                indicator = Indicator.Run(IconsService, port, Remove);
            }
            catch (Exception e)
            {
                return e.Message;
            }

            Indicators.Add(indicator);

            return null;
        }

        public void Remove(Indicator indicator)
        {
            indicator.Quit();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Indicators.Remove(indicator);
            });
        }

        public void ShutDownAll()
        {
            Indicators.ToList().ForEach(i => i.Quit());
        }
    }
}
