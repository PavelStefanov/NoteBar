using System;
using System.ServiceModel;

namespace Notebar.WCF
{
    public class NotebarServiceHost : ServiceHost
    {
        public NotebarServiceHost(Func<uint, string> addFnc, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            foreach (var cd in ImplementedContracts.Values)
            {
                cd.Behaviors.Add(new NotebarServiceInstanceProvider(addFnc));
            }
        }
    }
}
