using System;
using System.ServiceModel;

namespace NoteBar.WCF
{
    public class NoteBarServiceHost : ServiceHost
    {
        public NoteBarServiceHost(Func<uint, string> addFnc, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            foreach (var cd in ImplementedContracts.Values)
            {
                cd.Behaviors.Add(new NoteBarServiceInstanceProvider(addFnc));
            }
        }
    }
}
