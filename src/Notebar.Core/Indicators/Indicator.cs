using System;
using System.Diagnostics;
using Notebar.Core.Icons;
using Notebar.Core.Udp;

namespace Notebar.Core.Indicators
{
    public class Indicator
    {
        private UdpServer UdpServer { get; }
        private IconsService IconsService { get; }
        private Action<Indicator> QuitFnc { get; }

        public string ImagePath { get; private set; }
        public uint Port { get; private set; }

        public Indicator(IconsService iconsService, uint port, Action<Indicator> quitFnc)
        {
            IconsService = iconsService ?? throw new System.ArgumentNullException(nameof(iconsService));
            QuitFnc = quitFnc;

            UdpServer = new UdpServer(port, OnGetMessage);
        }

        public static Indicator Run(IconsService iconsService, uint port, Action<Indicator> quitFnc)
        {
            var indicator = new Indicator(iconsService, port, quitFnc);
            indicator.UdpServer.Start();

            return indicator;
        }

        private void OnGetMessage(string message)
        {
            if (message == "quit")
            {
                QuitFnc?.Invoke(this);
            }

            var icon = IconsService.FindIcon(message);
            if (icon == null)
            {
                EventLog.WriteEntry("Notebar", $"Cannot find '{icon}' icon", EventLogEntryType.Warning);
                ImagePath = IconsService.FindIcon("question");
                return;
            }

            ImagePath = icon;
        }

        public void Quit()
        {
            UdpServer.ShutDown();
        }
    }
}
