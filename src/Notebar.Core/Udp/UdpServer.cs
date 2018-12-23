using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Notebar.Core.Udp
{
    public class UdpServer
    {
        private UdpClient UdpClient { get; }
        private Action<string> GetMessageAction { get; }

        public UdpServer(uint port, Action<string> getMessageAction)
        {
            UdpClient = new UdpClient((int)port);
            GetMessageAction = getMessageAction;
        }

        public UdpServer Start()
        {
            BeginReceive();
            return this;
        }

        private void BeginReceive()
        {
            UdpClient.BeginReceive(HandleData, null);
        }

        private void HandleData(IAsyncResult result)
        {
            var iPEndPoint = new IPEndPoint(IPAddress.Any, 0);

            try
            {
                var data = UdpClient.EndReceive(result, ref iPEndPoint);
                var message = Encoding.UTF8.GetString(data);
                GetMessageAction?.Invoke(message);
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            BeginReceive();
        }

        public void ShutDown()
        {
            UdpClient.Close();
        }

        public static UdpServer Run(uint port, Action<string> getMessageAction)
        {
            return new UdpServer(port, getMessageAction).Start();
        }
    }
}
