using System;
using System.Collections.ObjectModel;
using System.Linq;
using NoteBar.Core.Icons;

namespace NoteBar.Core.Indicators
{
    public class IndicatorsService
    {
        public readonly ObservableCollection<Indicator> Indicators = new ObservableCollection<Indicator>();

        public IconsService IconsService { get; }
        public Action<Indicator> RemoveFnc { get; }

        public IndicatorsService(IconsService iconsService, Action<Indicator> removeFnc)
        {
            IconsService = iconsService ?? throw new ArgumentNullException(nameof(iconsService));
            RemoveFnc = removeFnc;
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
                indicator = Indicator.Run(IconsService, port, RemoveFnc);
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
            Indicators.Remove(indicator);
        }

        public void ShutDownAll()
        {
            Indicators.ToList().ForEach(i => i.Quit());
        }
    }
}
