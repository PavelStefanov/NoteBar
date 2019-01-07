using System.Windows.Interop;

namespace Notebar.Toolbar.Helpers
{
    public class DpiOptions
    {
        public double HeightFactor { get; set; }
        public double WidthFactor { get; set; }
    }

    public static class DpiHelper
    {
        public static DpiOptions GetDpiOptions()
        {
            using (var source = new HwndSource(new HwndSourceParameters()))
            {
                var transformToDevice = source.CompositionTarget.TransformToDevice;

                return new DpiOptions
                {
                    WidthFactor = transformToDevice.M11,
                    HeightFactor = transformToDevice.M22
                };
            }
        }
    }
}
