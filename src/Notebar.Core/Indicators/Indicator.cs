using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Notebar.Core.Icons;
using Notebar.Core.Udp;

namespace Notebar.Core.Indicators
{
    public class Indicator : INotifyPropertyChanged
    {
        private UdpServer UdpServer { get; }
        private IconsService IconsService { get; }
        private Action<Indicator> QuitFnc { get; }

        public uint Port { get; }

        private string _imagePath;
        public string ImagePath
        {
            get => _imagePath;
            private set
            {
                if (value == _imagePath)
                    return;

                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public Indicator(IconsService iconsService, uint port, Action<Indicator> quitFnc)
        {
            IconsService = iconsService ?? throw new ArgumentNullException(nameof(iconsService));
            Port = port;
            QuitFnc = quitFnc;

            UdpServer = new UdpServer(port, OnGetMessage);
            ImagePath = iconsService.FindIcon("white");
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
                return;
            }

            var icon = IconsService.FindIcon(message);
            if (icon == null)
            {
                EventLog.WriteEntry("Notebar", $"Cannot find '{icon}' icon", EventLogEntryType.Warning);
                icon = IconsService.FindIcon("question");
            }

            ImagePath = icon;
        }

        public void Quit()
        {
            UdpServer.ShutDown();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
