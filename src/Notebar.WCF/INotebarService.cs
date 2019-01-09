using System.ServiceModel;

namespace NoteBar.WCF
{
    [ServiceContract]
    public interface INoteBarService
    {
        [OperationContract]
        string AddIndicator(uint port);
    }
}
