using System;

namespace NoteBar.WCF
{
    public class NoteBarService : INoteBarService
    {
        public Func<uint, string> AddFnc { get; }

        public NoteBarService(Func<uint, string> addFnc)
        {
            AddFnc = addFnc;
        }

        public string AddIndicator(uint port)
        {
            return AddFnc?.Invoke(port);
        }
    }
}
