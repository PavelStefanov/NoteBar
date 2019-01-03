using CSDeskBand;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Notebar.Toolbar
{
    public static class OrientationHelper
    {

        public static Orientation Map(TaskbarOrientation taskbarOrientation)
        {
            var orientations = InitOrientationsDict();
            return orientations[taskbarOrientation];
        }

        private static Dictionary<TaskbarOrientation, Orientation> InitOrientationsDict() =>
            new Dictionary<TaskbarOrientation, Orientation>
            {
                { TaskbarOrientation.Horizontal, Orientation.Horizontal },
                { TaskbarOrientation.Vertical, Orientation.Vertical }
            };
    }
}
