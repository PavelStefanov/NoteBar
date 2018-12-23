using System;
using System.Collections.Generic;
using Notebar.Core.Icons;

namespace Notebar.Core.Indicators
{
    public class IndicatorsService
    {
        private Dictionary<uint, Indicator> Indicators = new Dictionary<uint, Indicator>();

        public IconsService IconsService { get; }

        public IndicatorsService(IconsService iconsService)
        {
            IconsService = iconsService ?? throw new ArgumentNullException(nameof(iconsService));
        }

        public string Add(uint port)
        {
            if (Indicators.ContainsKey(port))
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

            Indicators.Add(port, indicator);

            return null;
        }

        private void Remove(Indicator indicator)
        {
            indicator.Quit();
            Indicators.Remove(indicator.Port);
        }
    }
}
