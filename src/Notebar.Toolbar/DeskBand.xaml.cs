using CSDeskBand;
using CSDeskBand.ContextMenu;
using Grpc.Core;
using Notebar.gRPC;
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

namespace Notebar.Toolbar
{
    [ComVisible(true)]
    [Guid("32da9796-4495-4157-9aac-d1d7564c4119")]
    [CSDeskBandRegistration(Name = "Notebar", ShowDeskBand = true)]
    public partial class DeskBand
    {
        public DeskBand()
        {
            InitializeComponent();
            Options.MinHorizontalSize = new Size(20, 20);
            Options.MinVerticalSize = new Size(20, 20);

            //Port = defaultPort;

            //Options.ContextMenuItems = GetContextMenuItems(Port);

        }

        private List<DeskBandMenuItem> GetContextMenuItems(int port)
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
                    Text = $"UDP port: {port}"
                },
               quitAction
            };
        }

        private void Quit(object sender, System.Windows.RoutedEventArgs e)
        {
            CloseDeskBand();
            //Server.ShutdownAsync().Wait();
        }

        protected override void OnClose()
        {
            base.OnClose();
            //Server.ShutdownAsync().Wait();
        }
    }
}
