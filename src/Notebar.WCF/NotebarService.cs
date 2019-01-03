using System;

namespace Notebar.WCF
{
    public class NotebarService : INotebarService
    {
        public Func<uint, string> AddFnc { get; }

        public NotebarService(Func<uint, string> addFnc)
        {
            AddFnc = addFnc;
        }

        public string AddIndicator(uint port)
        {
            return AddFnc?.Invoke(port);
        }
    }
}
