using System.ServiceModel;

namespace Notebar.WCF
{
    [ServiceContract]
    public interface INotebarService
    {
        [OperationContract]
        string AddIndicator(uint port);
    }
}
