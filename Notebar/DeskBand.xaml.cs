using CSDeskBand;
using CSDeskBand.ContextMenu;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Notebar
{
    [ComVisible(true)]
    [Guid("32da9796-4495-4157-9aac-d1d7564c4119")]
    [CSDeskBandRegistration(Name = "Notebar", ShowDeskBand = true)]
    public partial class DeskBand : INotifyPropertyChanged
    {
        private const int defaultPort = 1738;

        private string _imagePath;

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                if (value == _imagePath)
                    return;

                _imagePath = value;
                OnPropertyChanged();
            }
        }

        private string[] DefaultIcons { get; set; }

        public DeskBand()
        {
            InitializeComponent();
            Options.MinHorizontalSize = new Size(20, 20);
            Options.MinVerticalSize = new Size(20, 20);
            Options.ContextMenuItems = GetContextMenuItems();

            DefaultIcons = GetDefaultIcons();
            SetIcon("white");

            RunServer();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<DeskBandMenuItem> GetContextMenuItems()
        {
            var quitAction = new DeskBandMenuAction("Quit")
            {
                Text = "Quit",
            };
            quitAction.Clicked += (sender, args) => CloseDeskBand();

            return new List<DeskBandMenuItem>() {
                new DeskBandMenuAction("Port")
                {
                    Enabled = false,
                    Text = $"UDP port: {defaultPort}"
                },
               quitAction
            };
        }

        private void RunServer()
        {
            Task.Run(() =>
            {
                using (var udpClient = new UdpClient(defaultPort))
                {
                    var RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                    try
                    {
                        while (true)
                        {
                            var bytes = udpClient.Receive(ref RemoteIpEndPoint);
                            var message = Encoding.UTF8.GetString(bytes);
                            HandleMessage(message);
                        }
                    }
                    catch (Exception e)
                    {
                        EventLog.WriteEntry("Notebar", e.ToString(), EventLogEntryType.Warning);
                    }
                }
            });
        }

        private void HandleMessage(string message)
        {
            if (message == "quit")
            {
                CloseDeskBand();
            }

            SetIcon(message);
        }

        private void SetIcon(string icon)
        {
            var image = FindIconInAppData(icon);
            if (image != null)
            {
                ImagePath = image;
                return;
            }

            image = FindIconInResource(icon);
            if (image != null)
            {
                ImagePath = image;
                return;
            }

            SetIcon("question");
            EventLog.WriteEntry("Notebar", $"Cannot find image '{icon}'", EventLogEntryType.Warning);
        }

        private string FindIconInAppData(string icon)
        {
            var appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Notebar");

            var paths = new[]
            {
                Path.Combine(appData, $"{icon}_alt@2x.png"),
                Path.Combine(appData, $"{icon}_alt.png"),
                Path.Combine(appData, $"{icon}@2x.png"),
                Path.Combine(appData, $"{icon}.png")
            };

            foreach (var path in paths)
            {
                if (File.Exists(path))
                {
                    return path;
                }
            }

            return null;
        }

        private string FindIconInResource(string icon)
        {
            var paths = new[]
            {
                $"Icons/{icon}_alt@2x.png",
                $"Icons/{icon}_alt.png",
                $"Icons/{icon}@2x.png",
                $"Icons/{icon}.png"
            };

            foreach (var path in paths)
            {
                if (DefaultIcons.Any(v => v.Equals(path, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return $"pack://application:,,,/Notebar;component/{path}";
                }
            }

            return null;
        }

        private static string[] GetDefaultIcons()
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resName = $"{assembly.GetName().Name}.g.resources";
            using (var stream = assembly.GetManifestResourceStream(resName))
            {
                using (var reader = new System.Resources.ResourceReader(stream))
                {
                    return reader.Cast<DictionaryEntry>().Select(entry => (string)entry.Key).ToArray();
                }
            }
        }
    }
}
