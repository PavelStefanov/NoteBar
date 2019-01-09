using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NoteBar.Core.Icons
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
            var appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NoteBar");
            var iconPath = Path.Combine(appData, $"{name}.png");

            return File.Exists(iconPath) ? iconPath : null;
        }

        private string FindIconInResource(string name)
        {
            var iconPath = $"Icons/Resources/{name}.png";

            return DefaultIcons.Any(i => i.Equals(iconPath, StringComparison.InvariantCultureIgnoreCase)) ?
                $"pack://application:,,,/NoteBar.Core;component/{iconPath}" : null;
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
