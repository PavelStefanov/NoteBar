using CSDeskBand.Interop.COM;
using System;
using System.Runtime.InteropServices;

namespace NoteBar.DeskBand
{
    public class TrayDeskband : IDisposable
    {
        private const int S_OK = 0;
        private const int S_FALSE = 1;

        private Type TrayDeskbandType { get; }
        private ITrayDeskband Instance { get; }

        public TrayDeskband()
        {
            TrayDeskbandType = Type.GetTypeFromCLSID(new Guid("E6442437-6C68-4f52-94DD-2CFED267EFB9"));
            Instance = Activator.CreateInstance(TrayDeskbandType) as ITrayDeskband;
        }

        public void Dispose()
        {
            if (Instance != null && Marshal.IsComObject(Instance))
            {
                Marshal.ReleaseComObject(Instance);
            }
        }

        public bool IsDeskBandShown(Guid clsid) =>
            Instance.IsDeskBandShown(ref clsid) == S_OK ? true : false;

        public bool ShowDeskBand(Guid clsid)
        {
            Instance.DeskBandRegistrationChanged();
            var result = Instance.ShowDeskBand(ref clsid) == S_OK;
            Instance.DeskBandRegistrationChanged();

            return result;
        }
    }
}
