using NoteBar.WCF;
using System;
using System.ServiceModel;

namespace NoteBar.Core.WCF
{
    public class WcfHost
    {
        private NoteBarServiceHost Host { get; set; }

        public static WcfHost Run(Func<uint, string> addFnc)
        {
            return new WcfHost().Start(addFnc);
        }

        public WcfHost Start(Func<uint, string> addFnc)
        {
            Host = new NoteBarServiceHost(addFnc, typeof(NoteBarService), new Uri("net.pipe://127.0.0.1/NoteBar"));

            var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            Host.AddServiceEndpoint(typeof(INoteBarService), binding, "NoteBarService");

            Host.Open();

            return this;
        }

        public void ShutDown()
        {
            Host.Close();
        }
    }
}
