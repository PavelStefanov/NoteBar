using Notebar.WCF;
using System;
using System.ServiceModel;

namespace Notebar.Core.WCF
{
    public class WcfHost
    {
        private NotebarServiceHost Host { get; set; }

        public static WcfHost Run(Func<uint, string> addFnc)
        {
            return new WcfHost().Start(addFnc);
        }

        public WcfHost Start(Func<uint, string> addFnc)
        {
            Host = new NotebarServiceHost(addFnc, typeof(NotebarService), new Uri("net.pipe://127.0.0.1/Notebar"));

            var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            Host.AddServiceEndpoint(typeof(INotebarService), binding, "NotebarService");

            Host.Open();

            return this;
        }

        public void ShutDown()
        {
            Host.Close();
        }
    }
}
