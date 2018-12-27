using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Notebar.Core.Icons
{
    public class IconsService
    {
        private string[] DefaultIcons { get; set; }

        public IconsService()
        {
            DefaultIcons = GetDefaultIcons();
        }

        public string FindIcon(string name)
        {
            var image = FindIconInAppData(name);
            if (image != null)
            {
                return image;
            }

            image = FindIconInResource(name);
            if (image != null)
            {
                return image;
            }

            return null;
        }

        private string FindIconInAppData(string name)
        {
            var appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Notebar");

            var paths = new[]
            {
                Path.Combine(appData, $"{name}_alt@2x.png"),
                Path.Combine(appData, $"{name}_alt.png"),
                Path.Combine(appData, $"{name}@2x.png"),
                Path.Combine(appData, $"{name}.png")
            };

            foreach (var path in paths)
            {
                if (File.Exists(path))
                {
                    return path;
                }
            }

            return null;
        }

        private string FindIconInResource(string icon)
        {
            var paths = new[]
            {
                $"Icons/Resources/{icon}_alt@2x.png",
                $"Icons/Resources/{icon}_alt.png",
                $"Icons/Resources/{icon}@2x.png",
                $"Icons/Resources/{icon}.png"
            };

            foreach (var path in paths)
            {
                if (DefaultIcons.Any(v => v.Equals(path, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return $"pack://application:,,,/Notebar.Core;component/{path}";
                }
            }

            return null;
        }

        private string[] GetDefaultIcons()
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourcesName = $"{assembly.GetName().Name}.g.resources";
            using (var stream = assembly.GetManifestResourceStream(resourcesName))
            {
                using (var reader = new System.Resources.ResourceReader(stream))
                {
                    return reader.Cast<DictionaryEntry>().Select(entry => (string)entry.Key).ToArray();
                }
            }
        }
    }
}
