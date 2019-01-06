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

            return paths.FirstOrDefault(p => File.Exists(p));
        }

        private string FindIconInResource(string name)
        {
            var paths = new[]
            {
                $"Icons/Resources/{name}_alt@2x.png",
                $"Icons/Resources/{name}_alt.png",
                $"Icons/Resources/{name}@2x.png",
                $"Icons/Resources/{name}.png"
            };

            var path = paths.FirstOrDefault(p =>
                DefaultIcons.Any(v => v.Equals(p, StringComparison.InvariantCultureIgnoreCase)));

            return string.IsNullOrEmpty(path) ? null : $"pack://application:,,,/Notebar.Core;component/{path}";
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
